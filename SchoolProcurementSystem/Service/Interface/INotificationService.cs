using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface INotificationService
    {
        Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, CancellationToken ct);
        Task<GeneraicResponse> GetUnreadCountAsync(CancellationToken ct);
        Task<GeneraicResponse> MarkAsReadAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> MarkAllAsReadAsync(CancellationToken ct);
        Task CreateAsync(
            int userId,
            string title,
            string message,
            string type,
            string? entityType,
            int? entityId,
            string? redirectUrl,
            CancellationToken ct);
    }

}
