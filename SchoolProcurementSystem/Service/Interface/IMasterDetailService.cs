using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IMasterDetailService
    {
        Task<GeneraicResponse> GetByCategoryAsync(string category, CancellationToken ct = default);
    }
}
