using SchoolProcurement.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class SorContactMappingItem
    {
        public int ID { get; set; }

        public int SorContactMappingID { get; set; }
        public SorContactMapping SorContactMapping { get; set; } = null!;

        public int ProductID { get; set; }
        public Product Product { get; set; } = null!;

        public decimal Quantity { get; set; }
        public decimal? QuotedRate { get; set; }

        public string Status { get; set; } = "Submitted";
        public DateTime CreatedDate { get; set; }
    }
}
