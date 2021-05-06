using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.AspNetCore;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyServiceCollectionExtensions
    {
        public static IServiceCollection AddMulitTenancy(this IServiceCollection services, Action<MultiTenancyCoreOptions> action = null)
        {
            services.TryAdd(new ServiceCollection()
                .AddSingleton<ICurrentPrincipalAccessor, HttpContextCurrentPrincipalAccessor>()
                .AddSingleton<ITenantResolveResultAccessor, HttpContextTenantResolveResultAccessor>()
                .AddTransient<MultiTenancyMiddleware>()
            );

            return services.AddMulitTenancyCore(action)
                .AddHttpContextAccessor();
        }
    }
}
