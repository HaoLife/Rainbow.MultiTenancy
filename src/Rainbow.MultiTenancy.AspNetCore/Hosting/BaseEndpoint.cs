using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public abstract class BaseEndpoint : IEndpointHandler
    {
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated == false)
                return new AuthErrorResult();

            return await this.ProcessContentAsync(context);
        }

        public abstract Task<IEndpointResult> ProcessContentAsync(HttpContext context);


    }
}
