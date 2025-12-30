using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class SorChat
    {
        public int ID { get; set; }
        public int SORID { get; set; }
        public int BranchID { get; set; }
        public int SenderUserID { get; set; }

        public string Message { get; set; } = string.Empty;

        // NEW
        public int? ParentChatID { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Navigation
        public User SenderUser { get; set; } = null!;
        public SorChat? ParentChat { get; set; }
        public ICollection<SorChat> Replies { get; set; } = new List<SorChat>();
        public ICollection<SorChatAttachment> Attachments { get; set; } = new List<SorChatAttachment>();
    }


}
