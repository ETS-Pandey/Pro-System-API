using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Request;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;
using System.Security.Cryptography;

namespace SchoolProcurement.Api.Service
{
    public class ContactService : BaseService, IContactService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly ISmtpEmailService _smtpEmailService;

        public ContactService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ILogger<ContactService> logger,
            ISmtpEmailService smtpEmailService)
            : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
            _smtpEmailService = smtpEmailService;
        }

        protected int RequireBranch()
        {
            if (_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Admin user cannot perform branch-scoped operation.");

            return _currentUser.UserBranchId
                ?? throw new InvalidOperationException("Branch context required.");
        }

        private int BranchId => RequireBranch();

        #region Contact CRUD

        public async Task<GeneraicResponse> CreateAsync(CreateContactDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                ValidateContact(dto.Name, dto.EmailAddress);

                var emailNorm = dto.EmailAddress?.Trim().ToLower();

                if (!string.IsNullOrWhiteSpace(emailNorm))
                {
                    var exists = await _db.ContactDetails.AnyAsync(c =>
                        !c.IsDeleted &&
                        c.BranchID == BranchId &&
                        c.EmailAddress != null &&
                        c.EmailAddress.Trim().ToLower() == emailNorm, ct);

                    if (exists)
                        throw new InvalidOperationException("Email already exists in this branch");
                }

                var now = DateTime.UtcNow;

                var ent = new ContactDetail
                {
                    BranchID = BranchId,
                    Name = dto.Name.Trim(),
                    Address = dto.Address?.Trim(),
                    Postcode = dto.Postcode?.Trim(),
                    City = dto.City?.Trim(),
                    State = dto.State?.Trim(),
                    Country = dto.Country?.Trim(),
                    MobileNo = dto.MobileNo?.Trim(),
                    EmailAddress = dto.EmailAddress?.Trim().ToLower(),
                    UniqueString = GenerateSixDigitCode(),
                    IsDeleted = false,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = now,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = now
                };

                _db.ContactDetails.Add(ent);
                await _db.SaveChangesAsync(ct);

                return MapToDto(ent);
            },
            "Contact created successfully",
            "Failed to create contact");
        }

        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var ent = await _db.ContactDetails
                    .Include(c => c.Branch)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.ID == id &&
                        !c.IsDeleted &&
                        c.BranchID == BranchId, ct);

                if (ent == null)
                    throw new KeyNotFoundException("Contact not found");

                return MapToDto(ent);
            },
            "Contact found successfully",
            "Failed to fetch contact");
        }

        public async Task<GeneraicResponse> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 200);

                var q = _db.ContactDetails
                    .Include(c => c.Branch)
                    .Where(c => !c.IsDeleted && c.BranchID == BranchId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var s = search.Trim();
                    q = q.Where(c =>
                        c.Name.Contains(s) ||
                        c.EmailAddress!.Contains(s) ||
                        c.MobileNo!.Contains(s) ||
                        c.City!.Contains(s));
                }

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderBy(c => c.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(ct);

                return new PagedResult<ContactDetailDto>
                {
                    TotalCount = total,
                    Items = items.Select(MapToDto).ToList()
                };
            },
            "Contacts found successfully",
            "Failed to fetch contacts");
        }

        public async Task<GeneraicResponse> UpdateAsync(UpdateContactDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                ValidateContact(dto.Name, dto.EmailAddress);

                var ent = await _db.ContactDetails.FirstOrDefaultAsync(c =>
                    c.ID == dto.ID &&
                    c.BranchID == BranchId &&
                    !c.IsDeleted, ct);

                if (ent == null)
                    throw new KeyNotFoundException("Contact not found");

                var emailNorm = dto.EmailAddress?.Trim().ToLower();

                if (!string.IsNullOrWhiteSpace(emailNorm))
                {
                    var exists = await _db.ContactDetails.AnyAsync(c =>
                        !c.IsDeleted &&
                        c.BranchID == BranchId &&
                        c.ID != dto.ID &&
                        c.EmailAddress != null &&
                        c.EmailAddress.ToLower() == emailNorm, ct);

                    if (exists)
                        throw new InvalidOperationException("Email already exists in this branch");
                }

                ent.Name = dto.Name.Trim();
                ent.Address = dto.Address?.Trim();
                ent.Postcode = dto.Postcode?.Trim();
                ent.City = dto.City?.Trim();
                ent.State = dto.State?.Trim();
                ent.Country = dto.Country?.Trim();
                ent.MobileNo = dto.MobileNo?.Trim();
                ent.EmailAddress = dto.EmailAddress?.Trim();
                ent.UniqueString = GenerateSixDigitCode();
                ent.UpdatedBy = _currentUser.UserId;
                ent.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
                return MapToDto(ent);
            },
            "Contact updated successfully",
            "Failed to update contact");
        }

        public async Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                var ent = await _db.ContactDetails.FirstOrDefaultAsync(c =>
                    c.ID == id &&
                    c.BranchID == BranchId &&
                    !c.IsDeleted, ct);

                if (ent == null)
                    throw new KeyNotFoundException("Contact not found");

                ent.IsDeleted = true;
                ent.UpdatedBy = _currentUser.UserId;
                ent.UpdatedDate = DateTime.UtcNow;

                await _db.SaveChangesAsync(ct);
            },
            "Contact deleted successfully",
            "Failed to delete contact");
        }

        #endregion

        #region Helpers

        private static void ValidateContact(string name, string? email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new ArgumentException("Invalid email address");
        }

        private static ContactDetailDto MapToDto(ContactDetail c) => new()
        {
            ID = c.ID,
            BranchID = c.BranchID,
            BranchName = c.Branch?.Name ?? string.Empty,
            Name = c.Name,
            Address = c.Address,
            MobileNo = c.MobileNo,
            EmailAddress = c.EmailAddress,
            UniqueString = c.UniqueString
        };

        private static bool IsValidEmail(string? email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email!);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static string GenerateSixDigitCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            return ((BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF) % 1_000_000).ToString("D6");
        }

        #endregion
    }
}
