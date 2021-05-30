using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rainbow.MultiTenancy.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class MultiTenancyServiceMiddleware : IMiddleware
    {
        private readonly IEndpointRouter router;
        private readonly ILogger<MultiTenancyServiceMiddleware> logger;

        public MultiTenancyServiceMiddleware(
            IEndpointRouter router,
            ILogger<MultiTenancyServiceMiddleware> logger)
        {
            this.router = router;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpoint = router.Find(context);
            if (endpoint != null)
            {
                logger.LogInformation("Invoking tenant endpoint: {endpointType} for {url}", endpoint.GetType().FullName, context.Request.Path.ToString());

                var result = await endpoint.ProcessAsync(context);

                if (result != null)
                {
                    logger.LogTrace("Invoking result: {type}", result.GetType().FullName);
                    await result.ExecuteAsync(context);
                }

                return;
            }

            await next(context);
        }

    }
}