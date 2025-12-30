using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ServiceOrderRequest
    {
        public int ID { get; set; }
        public string UniqueString { get; set; } = default!;
        public int BranchID { get; set; }
        public int? DepartmentID { get; set; }
        public string? PurposeDescription { get; set; }
        public string? AdditionalJustification { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public int? UrgencyLevelID { get; set; }
        public string Status { get; set; } = "New";
        public int? CurrentAssignedUserID { get; set; }
        public decimal? ActualCost { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // navigations
        public Branch? Branch { get; set; }
        public MasterDetail? Department { get; set; }
        public MasterDetail? UrgencyLevel { get; set; }
        public User? CurrentAssignedUser { get; set; }

        public ICollection<ServiceOrderRequestItem> Items { get; set; } = new List<ServiceOrderRequestItem>();
        public ICollection<ServiceOrderRequestAttachment> Attachments { get; set; } = new List<ServiceOrderRequestAttachment>();
        public ICollection<ServiceOrderRequestAssignment> Assignments { get; set; } = new List<ServiceOrderRequestAssignment>();
    }
}
