using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IPurchaseOrderService
    {
        Task<GeneraicResponse> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);
        Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
        Task<GeneraicResponse> ReceiveAsync(ReceivePurchaseOrderDto dto, CancellationToken ct);
        Task<GeneraicResponse> AddPaymentAsync(AddPurchaseOrderPaymentDto dto, CancellationToken ct);
    }

}
