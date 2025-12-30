using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface ISorChatService
    {
        Task<GeneraicResponse> CreateAsync(SorChatCreateDto dto, IEnumerable<IFormFile>? files, CancellationToken ct);
        Task<GeneraicResponse> GetBySorAsync(int sorId, CancellationToken ct);
        Task<GeneraicResponse> DeleteAsync(int chatId, CancellationToken ct);
    }

}
