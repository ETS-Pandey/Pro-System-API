using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IBranchService
    {
        Task<GeneraicResponse> GetAllAsync(int page, int pageSize, CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> CreateAsync(CreateBranchDto dto, CancellationToken ct);
        Task<GeneraicResponse> UpdateAsync(UpdateBranchDto dto, CancellationToken ct);
        Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct);
    }
}
