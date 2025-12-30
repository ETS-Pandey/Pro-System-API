using Microsoft.AspNetCore.Identity.Data;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IUserService
    {
        Task<GeneraicResponse> GetAllAsync(int page, int pageSize, CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> CreateAsync(CreateUserDto dto, CancellationToken ct);
        Task<GeneraicResponse> UpdateAsync(UpdateUserDto dto, CancellationToken ct);
        Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> LoginAsync(LoginRequest req, CancellationToken ct);
    }
}
