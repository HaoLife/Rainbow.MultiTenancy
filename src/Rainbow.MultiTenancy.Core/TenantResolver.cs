using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MultiTenancyCoreOptions _options;

        public TenantResolver(IOptions<MultiTenancyCoreOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public async Task<TenantResolveResult> ResolveTenantIdOrNameAsync()
        {
            var result = new TenantResolveResult();

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new TenantResolveContext(serviceScope.ServiceProvider);

                foreach (var tenantResolver in _options.TenantResolvers)
                {
                    await tenantResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(tenantResolver.Name);

                    if (context.HasResolvedTenantOrHost())
                    {
                        result.TenantIdOrName = context.TenantIdOrName;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
