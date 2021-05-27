using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using Rainbow.MultiTenancy.IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityServerBuilderConfigurationExtensions
    {
        public static IIdentityServerBuilder AddTenantIdentityServerCore(this IIdentityServerBuilder builder)
        {
            builder.Services
                .AddTransient<ICurrentUser, IdentityServerCurrentUser>()
                .AddTransient<IClaimsService, TenantClaimsService>();
            return builder;
        }

    }
}
