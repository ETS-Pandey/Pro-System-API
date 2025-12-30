namespace SchoolProcurement.Api.Dtos
{
    public class SorChatDto
    {
        public int ID { get; set; }
        public int SenderUserID { get; set; }
        public string SenderName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        // Quote info
        public int? ParentChatID { get; set; }
        public string? ParentMessage { get; set; }
        public string? ParentSenderName { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<SorChatAttachmentDto> Attachments { get; set; } = new();
    }

    public class SorChatAttachmentDto
    {
        public int ID { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }

    public class SorChatCreateDto
    {
        public int SORID { get; set; }
        public string Message { get; set; } = string.Empty;

        // NEW
        public int? ParentChatID { get; set; }
    }
}
