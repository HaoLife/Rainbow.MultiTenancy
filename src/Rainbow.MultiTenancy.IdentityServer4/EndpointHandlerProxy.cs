using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServer4
{

    public class EndpointHandlerProxy : IEndpointHandler
    {
        private readonly IEndpointHandler proxy;

        public EndpointHandlerProxy(IEndpointHandler proxy, IServiceProvider provider)
        {
            this.proxy = proxy;
            Provider = provider;
        }

        public IServiceProvider Provider { get; }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var session = this.Provider.GetRequiredService<IUserSession>();
            var currentTenant = this.Provider.GetRequiredService<ICurrentTenant>();
            var user = await session.GetUserAsync();

            using (currentTenant.Change(user.FindTenantId()))
                return await this.proxy.ProcessAsync(context);
        }
    }
}
