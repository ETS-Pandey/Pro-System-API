using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class SorChatAttachment
    {
        public int ID { get; set; }
        public int SORChatID { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? ContentType { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public SorChat? SorChat { get; set; }
    }

}
