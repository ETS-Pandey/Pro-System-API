using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolProcurement.Infrastructure.Middleware
{
    public class GlobalResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalResponseMiddleware> _logger;

        public GlobalResponseMiddleware(
            RequestDelegate next,
            ILogger<GlobalResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // If controller already returned GenericResponse, do nothing
                if (context.Response.HasStarted)
                    return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new GeneraicResponse
                {
                    status = "error",
                    message = "Something went wrong. Please contact support.",
                    error_message = ex.Message
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
