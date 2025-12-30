using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class EmailSetupDetail
    {
        public int ID { get; set; }
        public int BranchID { get; set; }       // 0 = global default
        public string FromEmail { get; set; } = default!;
        public int Port { get; set; }
        public string Host { get; set; } = default!;
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }   // store encrypted in production
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
