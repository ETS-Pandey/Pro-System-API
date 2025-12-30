using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ServiceOrderRequestAssignment
    {
        public int ID { get; set; }
        public int SORID { get; set; }
        public int UserID { get; set; }   // assigned to
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }  // who performed the assignment
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ServiceOrderRequest? SOR { get; set; }
        public User? User { get; set; }
    }
}
