using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface IContactService
    {
        Task<GeneraicResponse> CreateAsync(CreateContactDto dto, CancellationToken ct = default);
        Task<GeneraicResponse?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<GeneraicResponse> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
        Task<GeneraicResponse> UpdateAsync(UpdateContactDto dto, CancellationToken ct = default);
        Task<GeneraicResponse> DeleteAsync(int id, CancellationToken ct = default); // soft delete
    }
}
