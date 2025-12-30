using Microsoft.AspNetCore.Http;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Auditing;
using SchoolProcurement.Infrastructure.Security;
using System.Diagnostics;

namespace SchoolProcurement.Infrastructure.Middleware
{
    public class ApiAuditMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiAuditMiddleware(RequestDelegate next) => _next = next;

        // Do NOT inject scoped services in constructor.
        // Use parameters on InvokeAsync to let DI resolve them per-request (scoped).
        public async Task InvokeAsync(HttpContext context, IAuditService auditService, ICurrentUserService currentUser)
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();

            var log = new AuditLog
            {
                EventType = "ApiRequest",
                CorrelationId = currentUser.CorrelationId,
                EventTime = DateTime.UtcNow,
                UserId = currentUser.UserId,
                UserName = currentUser.UserName,
                Route = context.Request.Path,
                HttpMethod = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                DurationMs = (int)sw.ElapsedMilliseconds,
                Source = "API",
                Summary = $"{context.Request.Method} {context.Request.Path}"
            };

            try { await auditService.LogApiRequestAsync(log); } catch { /* swallow */ }
        }
    }
}
