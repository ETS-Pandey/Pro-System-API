using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class PurchaseOrderPayment
    {
        public int ID { get; set; }

        public int PurchaseOrderID { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public string? ReferenceNo { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }

}
