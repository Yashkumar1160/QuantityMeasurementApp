using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace QuantityMeasurementAppServices.Middleware
{
    /// <summary>
    /// Middleware to handle Correlation IDs (Trace IDs) for distributed tracing.
    /// It ensures every request has a unique ID that can be passed between services.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Check if the request already has a Correlation ID (from Gateway or another service)
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out StringValues correlationId))
            {
                // 2. If not, generate a new one (GUID)
                correlationId = Guid.NewGuid().ToString();
            }

            // 3. Store it in HttpContext for logging and downstream usage
            context.Items[CorrelationIdHeader] = correlationId.ToString();

            // 4. Continue to the next middleware/controller
            await next(context);
        }
    }
}
