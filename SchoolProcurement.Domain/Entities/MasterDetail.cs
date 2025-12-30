using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class MasterDetail
    {
        public int ID { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? OtherName { get; set; }
        public string? Description { get; set; }
        public int? ParentID { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Optional self-reference
        public MasterDetail? Parent { get; set; }
        public ICollection<MasterDetail>? Children { get; set; }
    }
}
