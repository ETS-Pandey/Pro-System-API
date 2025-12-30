using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SchoolProcurement.Infrastructure.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next) => _next = next;
        public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUser)
        {
            var correlationHeader = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();
            Guid corrId = Guid.TryParse(correlationHeader, out var parsed)
                ? parsed
                : Guid.NewGuid();

            int? userId = null;
            int? branchId = null;
            string? userName = null;
            string? role = null;

            var user = context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                userId = int.TryParse(
                    user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub),
                    out var uid) ? uid : null;

                role = user.FindFirstValue("role");

                userName = user.FindFirstValue(ClaimTypes.Name)
                           ?? user.FindFirstValue("name");

                if (int.TryParse(user.FindFirstValue("branch"), out var bid))
                    branchId = bid;
            }

            currentUser.SetUser(userId, branchId, userName, role, corrId);

            await _next(context);
        }
    }
}
