using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class UserNotification
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public string NotificationType { get; set; } = null!;
        public string? EntityType { get; set; }
        public int? EntityID { get; set; }

        public string? RedirectUrl { get; set; }

        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

