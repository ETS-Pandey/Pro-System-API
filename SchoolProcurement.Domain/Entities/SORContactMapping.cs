using SchoolProcurement.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class SorContactMapping
    {
        public int ID { get; set; }

        public int SORID { get; set; }
        public ServiceOrderRequest SOR { get; set; } = null!;

        public int BranchID { get; set; }
        public Branch Branch { get; set; } = null!;

        public int ContactID { get; set; }
        public ContactDetail Contact { get; set; } = null!;

        public string UniqueString { get; set; } = null!;
        public string Status { get; set; } = "Invited";

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<SorContactMappingItem> Items { get; set; } = new List<SorContactMappingItem>();
        public ICollection<SorContactMappingAttachment> Attachments { get; set; } = new List<SorContactMappingAttachment>();
    }
}
