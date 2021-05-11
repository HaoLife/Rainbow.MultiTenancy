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


        public static MultiTenancyCoreOptions AddDomainTenantResolveContributor(this MultiTenancyCoreOptions options, string domain)
        {
            options.TenantResolvers.Add(new DomainTenantResolveContributor(domain));

            return options;
        }
    }
}
