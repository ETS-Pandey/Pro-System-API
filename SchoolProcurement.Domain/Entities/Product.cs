using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class Product
    {
        public int ID { get; set; }

        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        // FKs to MasterDetails
        public int CategoryID { get; set; }
        public int UnitTypeID { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal PurchasePrice { get; set; }

        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // navigation properties
        public MasterDetail? Category { get; set; }
        public MasterDetail? UnitType { get; set; }
    }
}
