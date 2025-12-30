using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class PurchaseOrderReceipt
    {
        public int ID { get; set; }

        public int PurchaseOrderID { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public DateTime ReceiptDate { get; set; }

        public string? Remarks { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public ICollection<PurchaseOrderReceiptItem> Items { get; set; } = new List<PurchaseOrderReceiptItem>();
    }

}
