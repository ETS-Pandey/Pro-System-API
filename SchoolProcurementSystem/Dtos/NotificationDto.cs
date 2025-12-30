namespace SchoolProcurement.Api.Dtos
{
    public class NotificationDto
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string NotificationType { get; set; } = null!;
        public string? EntityType { get; set; }
        public int? EntityID { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

