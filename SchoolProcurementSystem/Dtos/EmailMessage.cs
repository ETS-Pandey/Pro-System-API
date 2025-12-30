namespace SchoolProcurement.Api.Dtos
{
    public class EmailMessage
    {
        public string FromAddress { get; set; } = default!;
        public string FromName { get; set; } = default!;
        public List<string> To { get; set; } = new();
        public List<string> Cc { get; set; } = new();
        public List<string> Bcc { get; set; } = new();
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!; // HTML or plain text
        public bool IsBodyHtml { get; set; } = true;
        // Tuple of (filename, stream) for attachments (optional)
        public List<(string FileName, byte[] Content)> Attachments { get; set; } = new();
    }
}
