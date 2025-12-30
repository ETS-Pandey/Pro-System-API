using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ContactDetail
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public string Name { get; set; } = default!;
        public string? UniqueString { get; set; }
        public string? Address { get; set; }
        public string? Postcode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailAddress { get; set; }

        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Branch? Branch { get; set; }
    }
}
