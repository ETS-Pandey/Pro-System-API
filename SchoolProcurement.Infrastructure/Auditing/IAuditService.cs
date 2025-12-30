using SchoolProcurement.Domain.Entities;

namespace SchoolProcurement.Infrastructure.Auditing
{
    public interface IAuditService
    {
        Task LogApiRequestAsync(AuditLog log);
    }
}
