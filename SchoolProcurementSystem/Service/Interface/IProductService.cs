using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IProductService
    {
        Task<GeneraicResponse> GetAllPagedAsync(int page, int pageSize, int? categoryId, int? unitTypeId, string? search, CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> CreateAsync(Product product, CancellationToken ct);
        Task<GeneraicResponse> UpdateAsync(Product product, CancellationToken ct);
        Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct);
    }
}
