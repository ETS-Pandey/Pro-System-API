using Microsoft.AspNetCore.Http;

namespace SchoolProcurement.Infrastructure.Middleware
{
    public class RequestCorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Items.ContainsKey("CorrelationId"))
            {
                var id = Guid.NewGuid();
                context.Items["CorrelationId"] = id;
                context.Response.Headers["X-Correlation-ID"] = id.ToString();
            }

            await _next(context);
        }
    }
}
