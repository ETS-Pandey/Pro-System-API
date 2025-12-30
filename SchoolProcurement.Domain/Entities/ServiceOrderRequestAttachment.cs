using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class ServiceOrderRequestAttachment
    {
        public int ID { get; set; }
        public int SORID { get; set; }
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ServiceOrderRequest? SOR { get; set; }
    }
}
