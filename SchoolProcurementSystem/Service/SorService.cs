using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Constants;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class SorService : BaseService, ISorService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly INotificationService _notificationService;

        public SorService(SchoolDbContext db, ICurrentUserService currentUser,
            ILogger<SorService> logger, ISmtpEmailService smtpEmailService,
            INotificationService notificationService) : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
            _smtpEmailService = smtpEmailService;
            _notificationService = notificationService;
        }

        protected int RequireBranch()
        {
            if (_currentUser.IsAdmin)
                throw new UnauthorizedAccessException("Admin user cannot perform branch-scoped operation.");

            return _currentUser.UserBranchId
                ?? throw new InvalidOperationException("Branch context required.");
        }

        private int BranchId => RequireBranch();

        #region Create
        public async Task<GeneraicResponse> CreateAsync(CreateSorDto dto, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (dto.Items == null || !dto.Items.Any())
                    throw new Exception("At least one item is required.");

                var unique = await GenerateUniqueStringAsync(ct);
                var now = DateTime.UtcNow;

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                var procurementUser = _db.Users.Include(m => m.Role).Where(m => m.BranchID == BranchId && m.Role.Name == "Procurement" && m.IsDelete == false).FirstOrDefaultAsync();

                if (procurementUser == null)
                    throw new Exception("Failed to found the procurement user details.");

                var sor = new ServiceOrderRequest
                {
                    UniqueString = unique,
                    BranchID = BranchId,
                    DepartmentID = dto.DepartmentID,
                    PurposeDescription = dto.PurposeDescription,
                    AdditionalJustification = dto.AdditionalJustification,
                    RequiredByDate = dto.RequiredByDate,
                    UrgencyLevelID = dto.UrgencyLevelID,
                    Status = "New",
                    ActualCost = dto.Items.Sum(i => i.EstimatedCost) ?? 0,
                    CurrentAssignedUserID = procurementUser.Id,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = now,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = now
                };

                _db.ServiceOrderRequests.Add(sor);
                await _db.SaveChangesAsync(ct);

                foreach (var it in dto.Items)
                {
                    _db.ServiceOrderRequestItems.Add(new ServiceOrderRequestItem
                    {
                        SORID = sor.ID,
                        ProductID = it.ProductID,
                        UnitTypeID = it.UnitTypeID,
                        Quantity = it.Quantity,
                        EstimatedCost = it.EstimatedCost,
                        TechnicalSpecifications = it.TechnicalSpecifications,
                        CreatedBy = _currentUser.UserId,
                        CreatedDate = now,
                        UpdatedBy = _currentUser.UserId,
                        UpdatedDate = now
                    });
                }

                _db.ServiceOrderRequestAssignments.Add(new ServiceOrderRequestAssignment
                {
                    SORID = sor.ID,
                    UserID = procurementUser.Id,
                    Note = "Initial Assign",
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = DateTime.UtcNow
                });

                await _db.SaveChangesAsync(ct);
                await _notificationService.CreateAsync(
                    procurementUser.Id,
                    "SOR Assigned",
                    $"SOR {sor.UniqueString} has been assigned to you",
                    NotificationTypes.SorAssigned,
                    "SOR",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );

                await trx.CommitAsync(ct);
                return await BuildSorDtoAsync(sor.ID, ct);
            },
            "Service Order Request created successfully",
            "Failed to create Service Order Request");
        }
        #endregion

        #region Get By Id
        public async Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                return await BuildSorDtoAsync(id, ct);
            },
            "SOR details fetched successfully",
            "Failed to fetch SOR details");
        }

        public async Task<GeneraicResponse> GetAssignedUsersBySorIdAsync(int sorId, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                // Admin can access any SOR, branch users only their branch
                var sorQuery = _db.ServiceOrderRequests
                    .AsNoTracking()
                    .Where(s => s.ID == sorId && !s.IsDeleted);

                if (!_currentUser.IsAdmin)
                {
                    var branchId = RequireBranch();
                    sorQuery = sorQuery.Where(s => s.BranchID == branchId);
                }

                var sorExists = await sorQuery.AnyAsync(ct);
                if (!sorExists)
                    throw new KeyNotFoundException("Service Order Request not found.");

                var list = await _db.ServiceOrderRequestAssignments
                    .Include(a => a.User)
                    .AsNoTracking()
                    .Where(a => a.SORID == sorId)
                    .OrderByDescending(a => a.CreatedDate)
                    .Select(a => new SorAssignedUserDto
                    {
                        AssignmentID = a.ID,
                        UserID = a.UserID,
                        UserName = a.User.FirstName + " " + a.User.LastName,
                        Note = a.Note,
                        AssignedDate = a.CreatedDate
                    })
                    .ToListAsync(ct);

                return list;
            },
            "Assigned users fetched successfully",
            "Failed to fetch assigned users");
        }

        #endregion

        #region Paged
        public async Task<GeneraicResponse> GetPagedAsync(
            int page, int pageSize, int? assignedUserId = null, string? status = null, CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 200);

                var q = _db.ServiceOrderRequests
                    .AsNoTracking()
                    .Where(s => !s.IsDeleted && s.BranchID == BranchId);

                if (!string.IsNullOrWhiteSpace(status))
                    q = q.Where(s => s.Status == status);

                if (assignedUserId.HasValue)
                    q = q.Where(s => s.CurrentAssignedUserID == assignedUserId);

                var total = await q.CountAsync(ct);

                var items = await q
                    .OrderByDescending(s => s.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new SorDto
                    {
                        ID = s.ID,
                        UniqueString = s.UniqueString,
                        PurposeDescription = s.PurposeDescription,
                        BranchID = s.BranchID,
                        Status = s.Status,
                        CreatedDate = s.CreatedDate
                    })
                    .ToListAsync(ct);

                return new PagedResult<SorDto>
                {
                    TotalCount = total,
                    Items = items
                };
            },
            "SOR list fetched successfully",
            "Failed to fetch SOR list");
        }
        #endregion

        #region Assign / Reassign
        public async Task<GeneraicResponse> AssignAsync(int sorId, int toUserId, string? note, CancellationToken ct)
            => await ChangeAssignmentAsync(sorId, toUserId, note, "Assigned", ct);

        public async Task<GeneraicResponse> ReassignAsync(int sorId, int toUserId, string? note, CancellationToken ct)
            => await ChangeAssignmentAsync(sorId, toUserId, note, "Assigned", ct);

        // Add item to existing SOR
        public async Task<GeneraicResponse> AddItemAsync(
    int sorId,
    SorItemCreateDto item,
    CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (item == null)
                    throw new ArgumentException("Item details are required");

                var sor = await _db.ServiceOrderRequests
                    .FirstOrDefaultAsync(s =>
                        s.ID == sorId &&
                        !s.IsDeleted &&
                        s.BranchID == BranchId, ct);

                if (sor == null)
                    throw new KeyNotFoundException("Service Order Request not found");

                var curUserId = _currentUser.UserId
                    ?? throw new InvalidOperationException("Current user not available");

                // Permission check
                if (sor.CurrentAssignedUserID != curUserId && sor.CreatedBy != curUserId)
                    throw new UnauthorizedAccessException("Not authorized to add items to this SOR");

                // Validate product
                var prodExists = await _db.Products
                    .AnyAsync(p => p.ID == item.ProductID && !p.IsDeleted, ct);

                if (!prodExists)
                    throw new ArgumentException("Product not found");

                var now = DateTime.UtcNow;

                var ent = new ServiceOrderRequestItem
                {
                    SORID = sorId,
                    ProductID = item.ProductID,
                    UnitTypeID = item.UnitTypeID,
                    Quantity = item.Quantity,
                    EstimatedCost = item.EstimatedCost,
                    TechnicalSpecifications = item.TechnicalSpecifications,
                    IsDeleted = false,
                    CreatedBy = curUserId,
                    CreatedDate = now,
                    UpdatedBy = curUserId,
                    UpdatedDate = now
                };

                _db.ServiceOrderRequestItems.Add(ent);
                await _db.SaveChangesAsync(ct);

                return ent.ID;
            },
            "Item added to Service Order Request successfully",
            "Failed to add item to Service Order Request");
        }

        // Add attachment
        public async Task<GeneraicResponse> AddAttachmentAsync(
    int sorId,
    SorAttachmentCreateDto attachment,
    CancellationToken ct = default)
        {
            return await ExecuteAsync(async () =>
            {
                if (attachment == null)
                    throw new ArgumentException("Attachment details are required");

                var sor = await _db.ServiceOrderRequests
                    .FirstOrDefaultAsync(s =>
                        s.ID == sorId &&
                        !s.IsDeleted &&
                        s.BranchID == BranchId, ct);

                if (sor == null)
                    throw new KeyNotFoundException("Service Order Request not found");

                var curUserId = _currentUser.UserId
                    ?? throw new InvalidOperationException("Current user not available");

                // Permission check
                if (sor.CurrentAssignedUserID != curUserId && sor.CreatedBy != curUserId)
                    throw new UnauthorizedAccessException("Not authorized to add attachments to this SOR");

                if (string.IsNullOrWhiteSpace(attachment.FileName) ||
                    string.IsNullOrWhiteSpace(attachment.FilePath))
                    throw new ArgumentException("Invalid attachment data");

                var now = DateTime.UtcNow;

                var ent = new ServiceOrderRequestAttachment
                {
                    SORID = sorId,
                    FileName = attachment.FileName.Trim(),
                    FilePath = attachment.FilePath.Trim(),
                    IsDeleted = false,
                    CreatedBy = curUserId,
                    CreatedDate = now,
                    UpdatedBy = curUserId,
                    UpdatedDate = now
                };

                _db.ServiceOrderRequestAttachments.Add(ent);
                await _db.SaveChangesAsync(ct);

                return ent.ID;
            },
            "Attachment added to Service Order Request successfully",
            "Failed to add attachment to Service Order Request");
        }

        #endregion

        #region Approve / Reject
        public async Task<GeneraicResponse> ApproveAsync(int sorId, bool approve, string? note, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var sor = await _db.ServiceOrderRequests
                    .FirstOrDefaultAsync(s => s.ID == sorId && !s.IsDeleted, ct);

                if (sor == null)
                    throw new Exception("SOR not found");

                var branchAdmin = await _db.Users.Include(m => m.Role)
                    .Where(u => u.BranchID == BranchId && u.Role.Name == "BranchAdmin")
                    .FirstOrDefaultAsync(ct);

                if (branchAdmin == null)
                    throw new Exception("Branch Admin not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                sor.Status = approve ? "Approved" : "Rejected";
                sor.UpdatedBy = _currentUser.UserId;
                sor.UpdatedDate = DateTime.UtcNow;

                _db.ServiceOrderRequestAssignments.Add(new ServiceOrderRequestAssignment
                {
                    SORID = sor.ID,
                    UserID = _currentUser.UserId ?? 0,
                    Note = note,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = DateTime.UtcNow
                });

                await _db.SaveChangesAsync(ct);
                var Approve = approve ? "approved" : "rejected";

                await _notificationService.CreateAsync(
                    _currentUser.UserId ?? 0,
                    "SOR Approved",
                    $"SOR {sor.UniqueString} has been {Approve} by you",
                    NotificationTypes.SorApproved,
                    "SOR",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );
                await _notificationService.CreateAsync(
                    sor.CreatedBy ?? 0,
                    "SOR Approved",
                    $"SOR {sor.UniqueString} has been {Approve} by {sor.CreatedBy}",
                    NotificationTypes.SorApproved,
                    "SOR",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );
                await _notificationService.CreateAsync(
                    branchAdmin.ID,
                    "SOR Approved",
                    $"SOR {sor.UniqueString} has been {Approve} by {sor.CreatedBy}",
                    NotificationTypes.SorApproved,
                    "SOR",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );
                await trx.CommitAsync(ct);

                /* ============================
 * SEND EMAIL ON APPROVAL
 * ============================ */
                if (approve)
                {
                    try
                    {
                        var creator = await _db.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.ID == sor.CreatedBy && !u.IsDelete, ct);

                        if (creator != null && !string.IsNullOrWhiteSpace(creator.Email))
                        {
                            var mail = new EmailMessage
                            {
                                To = new List<string> { creator.Email },
                                Subject = $"Service Order Request Approved - {sor.UniqueString}",
                                Body = $@"
<p>Hello <strong>{creator.FirstName} {creator.LastName}</strong>,</p>

<p>Your Service Order Request <strong>{sor.UniqueString}</strong> has been 
<strong style='color:green;'>approved</strong>.</p>

<p><strong>Purpose:</strong> {sor.PurposeDescription}</p>
<p><strong>Status:</strong> Approved</p>

<p>You may now proceed with the next steps.</p>

<p>Regards,<br/>
<strong>School Procurement System</strong></p>",
                                IsBodyHtml = true
                            };

                            await _smtpEmailService.SendEmailAsync(
                                mail,
                                sor.BranchID,
                                ct
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to send SOR approval email. SORID={SORID}",
                            sor.ID
                        );
                        // ❗ Do NOT throw — approval already succeeded
                    }
                }

                return true;
            },
            approve ? "SOR approved successfully" : "SOR rejected successfully",
            "Failed to process approval");
        }
        #endregion

        #region Helpers
        private async Task<SorDto?> BuildSorDtoAsync(int sorId, CancellationToken ct)
        {
            var sor = await _db.ServiceOrderRequests
                .Include(s => s.Items).ThenInclude(i => i.Product)
                .Include(s => s.Items).ThenInclude(i => i.UnitType)
                .Include(s => s.Attachments)
                .Include(s => s.Assignments)
                .Include(s => s.Branch)
                .FirstOrDefaultAsync(s => s.ID == sorId && !s.IsDeleted, ct);

            if (sor == null) return null;

            return new SorDto
            {
                ID = sor.ID,
                UniqueString = sor.UniqueString,
                BranchID = sor.BranchID,
                BranchName = sor.Branch?.Name,
                PurposeDescription = sor.PurposeDescription,
                Status = sor.Status,
                CreatedDate = sor.CreatedDate,
                Items = sor.Items.Select(i => new SorItemDto
                {
                    ID = i.ID,
                    ProductID = i.ProductID,
                    ProductName = i.Product?.Name,
                    UnitTypeID = i.UnitTypeID,
                    UnitTypeName = i.UnitType?.Name,
                    Quantity = i.Quantity,
                    EstimatedCost = i.EstimatedCost
                }).ToList()
            };
        }

        private async Task<GeneraicResponse> ChangeAssignmentAsync(int sorId, int toUserId, string? note, string status, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var sor = await _db.ServiceOrderRequests.FirstOrDefaultAsync(s => s.ID == sorId, ct);
                if (sor == null) throw new Exception("SOR not found");

                var user = await _db.Users.FirstOrDefaultAsync(s => s.ID == toUserId, ct);
                if (user == null) throw new Exception("User not found");

                await using var trx = await _db.Database.BeginTransactionAsync(ct);

                sor.CurrentAssignedUserID = toUserId;
                sor.Status = status;
                sor.UpdatedBy = _currentUser.UserId;
                sor.UpdatedDate = DateTime.UtcNow;

                _db.ServiceOrderRequestAssignments.Add(new ServiceOrderRequestAssignment
                {
                    SORID = sorId,
                    UserID = toUserId,
                    Note = note,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = _currentUser.UserId,
                    UpdatedDate = DateTime.UtcNow
                });

                await _db.SaveChangesAsync(ct);
                await _notificationService.CreateAsync(
                    toUserId,
                    "SOR Assigned",
                    $"SOR {sor.UniqueString} has been assigned to you",
                    NotificationTypes.SorAssigned,
                    "SOR",
                    sor.ID,
                    $"/sor/{sor.ID}",
                    ct
                );
                await trx.CommitAsync(ct);

                return true;
            },
            "SOR assigned successfully",
            "Failed to assign SOR");
        }

        private async Task<string> GenerateUniqueStringAsync(CancellationToken ct)
        {
            var today = DateTime.UtcNow.Date;
            var count = await _db.ServiceOrderRequests
                .CountAsync(s => s.CreatedDate >= today, ct);

            return $"SOR-{DateTime.UtcNow:yyyyMMdd}-{count + 1:0000}";
        }
        #endregion
    }
}
