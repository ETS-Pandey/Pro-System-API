using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IProductStockService
    {
        Task<GeneraicResponse> GetByProductAsync(int productId, CancellationToken ct = default);
        Task<GeneraicResponse> GetByBranchAsync(int branchId, CancellationToken ct = default);
        Task<GeneraicResponse> GetAsync(int productId, CancellationToken ct = default);
        Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, int? productId, int? branchId, string? search, CancellationToken ct = default);
        Task<GeneraicResponse> AdjustStockAsync(ProductStockAdjustmentDto dto, CancellationToken ct = default);
        Task<GeneraicResponse> GetLowStockAsync(CancellationToken ct = default);
    }
}
