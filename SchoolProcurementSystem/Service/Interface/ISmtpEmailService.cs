using SchoolProcurement.Api.Dtos;

namespace SchoolProcurement.Api.Service.Interface
{
    public interface ISmtpEmailService
    {
        Task SendEmailAsync(EmailMessage message, int? branchId = null, CancellationToken ct = default);
    }
}
