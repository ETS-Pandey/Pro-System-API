using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IRoleService
    {
        Task<GeneraicResponse> GetAllAsync(CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> CreateAsync(CreateRoleDto dto, CancellationToken ct);
        Task<GeneraicResponse> UpdateAsync(UpdateRoleDto dto, CancellationToken ct);
        Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct);
    }
}
