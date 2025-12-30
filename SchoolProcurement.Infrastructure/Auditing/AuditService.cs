using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Persistence;

namespace SchoolProcurement.Infrastructure.Auditing
{
    public class AuditService : IAuditService
    {
        private readonly SchoolDbContext _db;

        public AuditService(SchoolDbContext db)
        {
            _db = db;
        }

        public async Task LogApiRequestAsync(AuditLog log)
        {
            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
