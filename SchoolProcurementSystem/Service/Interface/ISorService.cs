using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface ISorService
    {
        Task<GeneraicResponse> CreateAsync(CreateSorDto dto, CancellationToken ct = default);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct = default);
        Task<GeneraicResponse> GetAssignedUsersBySorIdAsync(int sorId, CancellationToken ct = default);

        Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, int? assignedUserId = null, string? status = null, CancellationToken ct = default);

        Task<GeneraicResponse> AssignAsync(int sorId, int toUserId, string? note = null, CancellationToken ct = default);
        Task<GeneraicResponse> ReassignAsync(int sorId, int toUserId, string? note = null, CancellationToken ct = default);
        Task<GeneraicResponse> ApproveAsync(int sorId, bool approve, string? note = null, CancellationToken ct = default);

        Task<GeneraicResponse> AddItemAsync(int sorId, SorItemCreateDto item, CancellationToken ct = default);
        Task<GeneraicResponse> AddAttachmentAsync(int sorId, SorAttachmentCreateDto attachment, CancellationToken ct = default);
    }
}
