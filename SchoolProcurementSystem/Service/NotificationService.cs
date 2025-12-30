using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using SchoolProcurement.Infrastructure.Services;

namespace SchoolProcurement.Api.Service
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly SchoolDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public NotificationService(
            SchoolDbContext db,
            ICurrentUserService currentUser,
            ILogger<NotificationService> logger)
            : base(logger)
        {
            _db = db;
            _currentUser = currentUser;
        }

        private int UserId =>
            _currentUser.UserId ?? throw new InvalidOperationException("User not found");

        public async Task CreateAsync(
            int userId,
            string title,
            string message,
            string type,
            string? entityType,
            int? entityId,
            string? redirectUrl,
            CancellationToken ct)
        {
            try
            {
                var ent = new UserNotification
                {
                    UserID = userId,
                    Title = title,
                    Message = message,
                    NotificationType = type,
                    EntityType = entityType,
                    EntityID = entityId,
                    RedirectUrl = redirectUrl,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                _db.UserNotifications.Add(ent);
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                page = Math.Max(1, page);
                pageSize = Math.Clamp(pageSize, 1, 50);

                var q = _db.UserNotifications
                    .Where(n => n.UserID == UserId)
                    .OrderByDescending(n => n.CreatedDate);

                var total = await q.CountAsync(ct);

                var items = await q.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(ct);

                return new PagedResult<NotificationDto>
                {
                    TotalCount = total,
                    Items = items.Select(MapToDto).ToList()
                };
            },
            "Notifications fetched successfully",
            "Failed to fetch notifications");
        }

        public async Task<GeneraicResponse> GetUnreadCountAsync(CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                return await _db.UserNotifications
                    .CountAsync(n => n.UserID == UserId && !n.IsRead, ct);
            },
            "Unread count fetched",
            "Failed to fetch unread count");
        }

        public async Task<GeneraicResponse> MarkAsReadAsync(int id, CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var n = await _db.UserNotifications
                    .FirstOrDefaultAsync(x => x.ID == id && x.UserID == UserId, ct);

                if (n == null) throw new KeyNotFoundException("Notification not found");

                n.IsRead = true;
                n.ReadDate = DateTime.UtcNow;
                await _db.SaveChangesAsync(ct);
            },
            "Notification marked as read",
            "Failed to mark notification as read");
        }

        public async Task<GeneraicResponse> MarkAllAsReadAsync(CancellationToken ct)
        {
            return await ExecuteAsync(async () =>
            {
                var list = await _db.UserNotifications
                    .Where(n => n.UserID == UserId && !n.IsRead)
                    .ToListAsync(ct);

                foreach (var n in list)
                {
                    n.IsRead = true;
                    n.ReadDate = DateTime.UtcNow;
                }

                await _db.SaveChangesAsync(ct);
            },
            "All notifications marked as read",
            "Failed to mark notifications");
        }

        private static NotificationDto MapToDto(UserNotification n) => new()
        {
            ID = n.ID,
            Title = n.Title,
            Message = n.Message,
            NotificationType = n.NotificationType,
            EntityType = n.EntityType,
            EntityID = n.EntityID,
            RedirectUrl = n.RedirectUrl,
            IsRead = n.IsRead,
            CreatedDate = n.CreatedDate
        };
    }
}
