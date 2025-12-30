using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolProcurement.Infrastructure.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        public int? UserId { get; private set; }
        public int? UserBranchId { get; private set; }
        public string? UserName { get; private set; }
        public string? Role { get; private set; }
        public Guid CorrelationId { get; private set; } = Guid.Empty;

        public bool IsAdmin =>
            Role == "Admin" || Role == "SuperAdmin";

        public void SetUser(
            int? userId,
            int? branchId,
            string? userName,
            string? role,
            Guid? correlationId = null
        )
        {
            UserId = userId;
            UserBranchId = branchId;
            UserName = userName;
            Role = role;

            if (correlationId.HasValue && correlationId.Value != Guid.Empty)
                CorrelationId = correlationId.Value;
            else if (CorrelationId == Guid.Empty)
                CorrelationId = Guid.NewGuid();
        }
    }
}

