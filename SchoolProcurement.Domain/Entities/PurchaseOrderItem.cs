using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class PurchaseOrderItem
    {
        public int ID { get; set; }

        public int PurchaseOrderID { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public int ProductID { get; set; }
        public Product? Product { get; set; }

        public decimal OrderedQty { get; set; }
        public decimal ReceivedQty { get; set; }

        public decimal UnitPrice { get; set; }
        //public decimal TotalPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

