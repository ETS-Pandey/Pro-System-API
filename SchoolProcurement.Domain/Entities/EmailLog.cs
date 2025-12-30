using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class EmailLog
    {
        public int ID { get; set; }
        public int? BranchID { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? ToEmail { get; set; }    // comma-separated recipients
        public string? FromEmail { get; set; }
        public DateTime? SentDate { get; set; }
        public int TryCount { get; set; }
        public bool IsSent { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
