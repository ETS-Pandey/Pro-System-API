using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected readonly ILogger _logger;

        protected BaseService(ILogger logger)
        {
            _logger = logger;
        }

        protected GeneraicResponse Success(
            string message,
            object? data = null)
        {
            return new GeneraicResponse
            {
                status = "success",
                message = message,
                data = data
            };
        }

        protected GeneraicResponse Error(
            string message,
            Exception? ex = null)
        {
            if (ex != null)
                _logger.LogError(ex, message);

            return new GeneraicResponse
            {
                status = "error",
                message = message,
                error_message = ex?.ToString()
            };
        }

        protected async Task<GeneraicResponse> ExecuteAsync(
            Func<Task<object?>> action,
            string successMessage,
            string errorMessage)
        {
            try
            {
                var result = await action();
                return Success(successMessage, result);
            }
            catch (Exception ex)
            {
                return Error(errorMessage, ex);
            }
        }

        protected async Task<GeneraicResponse> ExecuteAsync(
            Func<Task> action,
            string successMessage,
            string errorMessage)
        {
            try
            {
                await action();
                return Success(successMessage);
            }
            catch (Exception ex)
            {
                return Error(errorMessage, ex);
            }
        }
    }
}
