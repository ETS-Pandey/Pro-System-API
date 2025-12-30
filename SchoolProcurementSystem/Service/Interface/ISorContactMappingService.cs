using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Infrastructure;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface ISorContactMappingService
    {
        Task<GeneraicResponse> InviteContactsAsync(InviteQuotationDto dto, CancellationToken ct);
        Task<GeneraicResponse> SubmitQuotationAsync(SubmitQuotationDto dto, CancellationToken ct);
        Task<GeneraicResponse> ApproveQuotationItemsAsync(ApproveQuotationItemsDto dto, CancellationToken ct);

        Task<GeneraicResponse> GetBySorAsync(int sorId, CancellationToken ct);
        Task<GeneraicResponse> GetByIdAsync(int id, CancellationToken ct);

        Task<GeneraicResponse> GenerateOTP(string email, int id, CancellationToken ct = default);
    //    Task<GeneraicResponse> SendQuotationEmail(
    //List<int> contacts,
    //int sorid,
    //CancellationToken ct = default);
    }
}
