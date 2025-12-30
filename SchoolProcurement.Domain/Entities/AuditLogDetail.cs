using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class AuditLogDetail
    {
        public long DetailId { get; set; }
        public long AuditId { get; set; }

        public string EntityName { get; set; } = default!;
        public string? PrimaryKey { get; set; }
        public string Operation { get; set; } = default!;
        public string? PropertyName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public AuditLog AuditLogs { get; set; } = default!;
    }
}
