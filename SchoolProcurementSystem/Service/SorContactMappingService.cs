using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Constants;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Constants;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;
using System.Buffers.Text;
using System.Security.Cryptography;

namespace SchoolProcurement.Api.Service
{
    public class SorContactMappingService : BaseService, ISorContactMappingService
    {
        private readonly SchoolDbContext _db;
        private readonly ISmtpEmailService _email;
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _notificationService;

        public SorContactMappingService(
            SchoolDbContext db,
            ISmtpEmailService email,
            ICurrentUserService currentUser,
            ILogger<SorContactMappingService> logger,
            INotificationService notificationService) : base(logger)
        {
            _db = db;
            _email = email;
            _currentUser = currentUser;
            _notificationService = notificationService;
        }

        private int BranchId =>
            _currentUser.UserBranchId
            ?? throw new InvalidOperationException("Branch not found");

        // --------------------------------------------------
        // 1. INVITE CONTACTS FOR QUOTATION
        // --------------------------------------------------
        public async Task<GeneraicResponse> InviteContactsAsync(
            InviteQuotationDto dto,
            CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var sor = await _db.ServiceOrderRequests
                    .FirstOrDefaultAsync(x => x.ID == dto.SORID && !x.IsDeleted, ct);

                if (sor == null)
                    throw new KeyNotFoundException("SOR not found");

                var branchAdmin = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == BranchId && u.Role.Name == "BranchAdmin")
                    .FirstOrDefaultAsync(ct);

                if (branchAdmin == null)
                    throw new Exception("Branch Admin not found");

                foreach (var contactId in dto.ContactIDs.Distinct())
                {
                    var exists = await _db.SorContactMappings.AnyAsync(x =>
                        x.SORID == dto.SORID &&
                        x.ContactID == contactId &&
                        !x.IsDeleted, ct);

                    if (exists) continue;

                    var mapping = new SorContactMapping
                    {
                        SORID = dto.SORID,
                        BranchID = BranchId,
                        ContactID = contactId,
                        UniqueString = Guid.NewGuid().ToString("N"),
                        Status = "Invited",
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = DateTime.UtcNow
                    };

                    _db.SorContactMappings.Add(mapping);
                    await _db.SaveChangesAsync(ct);

                    // 🔔 Send email invite
                    var contact = await _db.ContactDetails.FindAsync(contactId);
                    if (!string.IsNullOrWhiteSpace(contact?.EmailAddress))
                    {
                        await _notificationService.CreateAsync(
                            branchAdmin.ID,
                            "SOR Quotation Invited",
                            $"Quotation request for SOR {sor.UniqueString} has been sent to {contact.Name}.",
                            NotificationTypes.Invited,
                            "SORQuotationInvited",
                            sor.ID,
                            $"/sor/{sor.ID}",
                            ct
                        );

                        var baseUrl = AppConfig.Get("FrontConfig:BaseUrl");
                        var link = $"{baseUrl}?map={mapping.ID}";

                        await _email.SendEmailAsync(new EmailMessage
                        {
                            To = new() { contact.EmailAddress! },
                            Subject = "Submit the quotation",
                            Body = $@"
                    <p>Please provide quotation for the service request.</p>
                    <p>
                        <a href='{link}' target='_blank'>Click here</a>
                    </p>
                    <p>If link doesn’t work, copy:</p>
                    <p>{link}</p>
                    <p>Regards,<br/>School Procurement System</p>",
                            IsBodyHtml = true
                        }, sor.BranchID, ct);
                    }
                }

                await trx.CommitAsync(ct);
            },
            "Quotation invitations sent successfully",
            "Failed to send quotation invitations");
        }

        // --------------------------------------------------
        // 2. SUBMIT QUOTATION (VENDOR)
        // --------------------------------------------------
        public async Task<GeneraicResponse> SubmitQuotationAsync(
            SubmitQuotationDto dto,
            CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var branchAdmin = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == BranchId && u.Role.Name == "BranchAdmin")
                    .FirstOrDefaultAsync(ct);

                if (branchAdmin == null)
                    throw new Exception("Branch Admin not found");

                var procurementUser = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == BranchId && u.Role.Name == "Procurement")
                    .FirstOrDefaultAsync(ct);

                if (procurementUser == null)
                    throw new Exception("Procurement User not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var mapping = await _db.SorContactMappings
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x =>
                        x.UniqueString == dto.Token &&
                        !x.IsDeleted, ct);

                if (mapping == null)
                    throw new UnauthorizedAccessException("Invalid quotation token");

                var sor = await _db.ServiceOrderRequests
                    .FirstOrDefaultAsync(s => s.ID == mapping.SORID && !s.IsDeleted, ct);

                if (sor == null)
                    throw new Exception("SOR not found");

                foreach (var item in dto.Items)
                {
                    var entry = new SorContactMappingItem
                    {
                        SorContactMappingID = mapping.ID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        QuotedRate = item.QuotedRate,
                        Status = "Submitted",
                        CreatedDate = DateTime.UtcNow
                    };
                    _db.SorContactMappingItems.Add(entry);
                }

                mapping.Status = "Submitted";
                mapping.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                await _notificationService.CreateAsync(
                    branchAdmin.ID,
                    "SOR Submitted",
                    $"SOR {sor.UniqueString} has been submitted by {_currentUser.UserId}",
                    NotificationTypes.QuotationSubmitted,
                    "SORQuotation",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );

                await _notificationService.CreateAsync(
                    procurementUser.ID,
                    "SOR Submitted",
                    $"SOR {sor.UniqueString} has been submitted by {_currentUser.UserId}",
                    NotificationTypes.QuotationSubmitted,
                    "SORQuotation",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );

                await trx.CommitAsync(ct);
            },
            "Quotation submitted successfully",
            "Failed to submit quotation");
        }

        // --------------------------------------------------
        // 3. APPROVE MULTIPLE QUOTATION ITEMS
        // --------------------------------------------------
        public async Task<GeneraicResponse> ApproveQuotationItemsAsync(
            ApproveQuotationItemsDto dto,
            CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var branchAdmin = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == BranchId && u.Role.Name == "BranchAdmin")
                    .FirstOrDefaultAsync(ct);

                if (branchAdmin == null)
                    throw new Exception("Branch Admin not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var mapping = await _db.SorContactMappings
                    .Include(x => x.SOR)
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x =>
                        x.ID == dto.SorContactMappingID &&
                        !x.IsDeleted, ct);

                if (mapping == null)
                    throw new KeyNotFoundException("Quotation not found");

                if (mapping.SOR == null)
                    throw new KeyNotFoundException("SOR not found");

                foreach (var item in dto.Items)
                {
                    var ent = mapping.Items.FirstOrDefault(x => x.ID == item.SorContactMappingItemID);
                    if (ent == null) continue;

                    ent.Status = item.Approve ? "Approved" : "Rejected";
                }

                // Calculate overall status
                if (mapping.Items.All(x => x.Status == "Approved"))
                    mapping.Status = "FullyApproved";
                else if (mapping.Items.Any(x => x.Status == "Approved"))
                    mapping.Status = "PartiallyApproved";
                else
                    mapping.Status = "Submitted";

                mapping.UpdatedBy = _currentUser.UserId;
                mapping.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                await _notificationService.CreateAsync(
                    branchAdmin.ID,
                    "SOR Approved",
                    $"SOR {mapping.SOR.UniqueString} has been approved by {_currentUser.UserId}",
                    NotificationTypes.QuotationApproved,
                    "SORQuotation",
                    mapping.SORID,
                    $"/sor/{mapping.SORID}",
                    ct
                );

                await trx.CommitAsync(ct);
            },
            "Quotation items updated successfully",
            "Failed to approve quotation items");
        }

        // --------------------------------------------------
        // 4. GET QUOTATIONS BY SOR
        // --------------------------------------------------
        public async Task<GeneraicResponse> GetBySorAsync(int sorId, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                return await _db.SorContactMappings
                    .Include(x => x.Contact)
                    .Include(x => x.Items)
                    .Where(x => x.SORID == sorId && !x.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync(ct);
            },
            "Quotations fetched successfully",
            "Failed to fetch quotations");
        }

        // --------------------------------------------------
        // 5. GET QUOTATION BY ID
        // --------------------------------------------------
        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var data = await _db.SorContactMappings
                    .Include(x => x.Contact)
                    .Include(x => x.Items)
                    .Include(x => x.Attachments)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == id && !x.IsDeleted, ct);

                if (data == null)
                    throw new KeyNotFoundException("Quotation not found");

                return data;
            },
            "Quotation fetched successfully",
            "Failed to fetch quotation");
        }

        public async Task<GeneraicResponse> GenerateOTP(string email, int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email is required");

                var contact = await _db.SorContactMappings
                    .Include(m => m.Contact)
                    .FirstOrDefaultAsync(c =>
                        c.ID == id &&
                        !c.IsDeleted &&
                        c.Contact.EmailAddress != null &&
                        c.Contact.EmailAddress.ToLower() == email.Trim().ToLower(), ct);

                if (contact == null)
                    throw new KeyNotFoundException("Service details not found");

                contact.UniqueString = GenerateSixDigitCode();
                contact.UpdatedBy = _currentUser.UserId;
                contact.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);

                await _email.SendEmailAsync(new EmailMessage
                {
                    To = new() { contact.Contact.EmailAddress! },
                    Subject = "Your New OTP Code",
                    Body = $@"
                        <p>Hello <strong>{contact.Contact.Name}</strong>,</p>
                        <p>Your OTP code:</p>
                        <h2 style='color:#2a8bd5;'>{contact.UniqueString}</h2>
                        <p>Regards,<br/>School Procurement System</p>",
                    IsBodyHtml = true
                }, contact.BranchID, ct);

                return null;
            },
            "Authentication email sent successfully.",
            "Failed to send authentication email.");
        }

        //    public async Task<GeneraicResponse> SendQuotationEmail(
        //List<int> contacts,
        //int sorid,
        //CancellationToken ct = default)
        //    {
        //        return await ExecuteAsync(async () =>
        //        {
        //            await using var trx = await _db.Database.BeginTransactionAsync(ct);

        //            var sor = await _db.ServiceOrderRequests
        //                .FirstOrDefaultAsync(s => s.ID == sorid && !s.IsDeleted, ct);

        //            if (sor == null)
        //                throw new KeyNotFoundException("Service Order Request not found");

        //            var contactList = await _db.ContactDetails
        //                .Where(c => contacts.Contains(c.ID) && !c.IsDeleted)
        //                .ToListAsync(ct);

        //            if (!contactList.Any())
        //                throw new InvalidOperationException("No valid contacts found");

        //            var baseUrl = AppConfig.Get("FrontConfig:BaseUrl");

        //            foreach (var contact in contactList)
        //            {
        //                var mapping = new SorContactMapping
        //                {
        //                    BranchID = sor.BranchID,
        //                    SORID = sorid,
        //                    ContactID = contact.ID,
        //                    CreatedBy = _currentUser.UserId,
        //                    CreatedDate = DateTime.UtcNow
        //                };

        //                _db.SorContactMappings.Add(mapping);
        //                await _db.SaveChangesAsync(ct);

        //                var link = $"{baseUrl}?map={mapping.ID}";

        //                await _email.SendEmailAsync(new EmailMessage
        //                {
        //                    To = new() { contact.EmailAddress! },
        //                    Subject = "Submit the quotation",
        //                    Body = $@"
        //                <p>Please provide quotation for the service request.</p>
        //                <p>
        //                    <a href='{link}' target='_blank'>Click here</a>
        //                </p>
        //                <p>If link doesn’t work, copy:</p>
        //                <p>{link}</p>
        //                <p>Regards,<br/>School Procurement System</p>",
        //                    IsBodyHtml = true
        //                }, sor.BranchID, ct);
        //            }

        //            await trx.CommitAsync(ct);
        //            return null;
        //        },
        //        "Vendor quotation emails sent successfully.",
        //        "Failed to send quotation emails.");
        //    }

        private static string GenerateSixDigitCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            return ((BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF) % 1_000_000).ToString("D6");
        }
    }
}
