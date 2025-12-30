namespace SchoolProcurement.Domain.Entities
{
    public class AuditLog
    {
        public long AuditId { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;

        public string EventType { get; set; } = default!;
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Source { get; set; }
        public string? Route { get; set; }
        public string? HttpMethod { get; set; }
        public string? ClientIp { get; set; }
        public int? StatusCode { get; set; }
        public int? DurationMs { get; set; }
        public string? Summary { get; set; }
        public string? Extra { get; set; }

        public ICollection<AuditLogDetail> Details { get; set; } = new List<AuditLogDetail>();
    }
}
