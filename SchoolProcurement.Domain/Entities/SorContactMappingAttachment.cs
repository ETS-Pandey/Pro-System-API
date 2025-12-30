using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class SorContactMappingAttachment
    {
        public int ID { get; set; }

        public int SorContactMappingID { get; set; }
        public SorContactMapping SorContactMapping { get; set; } = null!;

        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;

        public DateTime CreatedDate { get; set; }
    }
}
