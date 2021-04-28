using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.AspNetCore;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyCoreOptionsExtensions
    {
        public static MultiTenancyCoreOptions AddHttpTenantResolveContributor(this MultiTenancyCoreOptions options)
        {
            options.TenantResolvers.AddRange(new ITenantResolveContributor[]
            {
                new QueryStringTenantResolveContributor(),
                new FormTenantResolveContributor(),
                new RouteTenantResolveContributor(),
                new HeaderTenantResolveContributor(),
                new CookieTenantResolveContributor(),
            });

            return options;
        }

    }
}
