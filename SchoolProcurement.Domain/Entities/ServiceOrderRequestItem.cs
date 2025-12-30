using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ServiceOrderRequestItem
    {
        public int ID { get; set; }
        public int SORID { get; set; }
        public int ProductID { get; set; }
        public int? UnitTypeID { get; set; }
        public decimal Quantity { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string? TechnicalSpecifications { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // navigations
        public ServiceOrderRequest? SOR { get; set; }
        public Product? Product { get; set; }
        public MasterDetail? UnitType { get; set; }
    }
}
