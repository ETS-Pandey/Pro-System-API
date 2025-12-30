namespace SchoolProcurement.Infrastructure.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        int? UserBranchId { get; }
        string? UserName { get; }
        string? Role { get; }
        Guid CorrelationId { get; }

        bool IsAdmin { get; }

        void SetUser(
            int? userId,
            int? branchId,
            string? userName,
            string? role,
            Guid? correlationId = null
        );
    }
}
