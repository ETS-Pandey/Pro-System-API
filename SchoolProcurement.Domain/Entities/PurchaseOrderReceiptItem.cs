using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class PurchaseOrderReceiptItem
    {
        public int ID { get; set; }

        public int ReceiptID { get; set; }
        public PurchaseOrderReceipt? Receipt { get; set; }

        public int PurchaseOrderItemID { get; set; }
        public PurchaseOrderItem? PurchaseOrderItem { get; set; }

        public decimal ReceivedQty { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }

}

