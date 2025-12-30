using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ProductStock
    {
        public int ID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int BranchID { get; set; }
        public Branch Branch { get; set; }

        public decimal Quantity { get; set; }
        public decimal ReservedQty { get; set; }
        public decimal? ReorderLevel { get; set; }

        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
