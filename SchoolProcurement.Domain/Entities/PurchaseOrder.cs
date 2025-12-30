using SchoolProcurement.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class PurchaseOrder
    {
        public int ID { get; set; }

        public string PONumber { get; set; } = null!;

        public int BranchID { get; set; }
        public Branch? Branch { get; set; }

        // ✅ NEW
        public int SupplierContactID { get; set; }
        public ContactDetail? SupplierContact { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }

        public string Status { get; set; } = "Open";
        public DateTime? ExpectedDeliveryDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<PurchaseOrderPayment> Payments { get; set; } = new List<PurchaseOrderPayment>();
        public ICollection<PurchaseOrderReceipt> Receipts { get; set; } = new List<PurchaseOrderReceipt>();
    }
}
