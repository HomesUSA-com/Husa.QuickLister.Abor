namespace Husa.Quicklister.Abor.Api.Middlewares
{
    using System;
    using System.Threading.Tasks;
    using Husa.Extensions.ServiceBus.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class RequestCorrelationIdMiddleware
    {
        private readonly RequestDelegate next;
        public RequestCorrelationIdMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task InvokeAsync(HttpContext context, IConfigureTraceId configureTraceId)
        {
            if (configureTraceId is null)
            {
                throw new ArgumentNullException(nameof(configureTraceId));
            }

            configureTraceId.SetTraceId();
            return this.next(context);
        }
    }
}
