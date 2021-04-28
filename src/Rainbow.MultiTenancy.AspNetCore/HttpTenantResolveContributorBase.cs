using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rainbow.MultiTenancy.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public abstract class HttpTenantResolveContributorBase : ITenantResolveContributor
    {
        public abstract string Name { get; }

        public async Task ResolveAsync(ITenantResolveContext context)
        {
            var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext == null)
            {
                return;
            }

            try
            {
                await ResolveFromHttpContextAsync(context, httpContext);
            }
            catch (Exception e)
            {
                context.ServiceProvider
                    .GetRequiredService<ILogger<HttpTenantResolveContributorBase>>()
                    .LogWarning(e.ToString());
            }
        }


        protected virtual async Task ResolveFromHttpContextAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantIdOrName = await GetTenantIdOrNameFromHttpContextOrNullAsync(context, httpContext);
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                context.TenantIdOrName = tenantIdOrName;
            }
        }

        protected abstract Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext);
    }
}
