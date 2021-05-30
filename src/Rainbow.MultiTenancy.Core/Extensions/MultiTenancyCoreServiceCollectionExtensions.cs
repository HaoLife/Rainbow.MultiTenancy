using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rainbow.MultiTenancy.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddMulitTenancyCore(this IServiceCollection services, Action<MultiTenancyCoreOptions> action = null)
        {
            services.TryAdd(new ServiceCollection()
                .AddSingleton<ICurrentPrincipalAccessor, ThreadCurrentPrincipalAccessor>()
                .AddSingleton<ITenantResolveResultAccessor, NullTenantResolveResultAccessor>()
                .AddSingleton<ICurrentTenantAccessor, AsyncLocalCurrentTenantAccessor>()
                .AddTransient<ICurrentUser, CurrentUser>()
                .AddTransient<ITenantStore, DefaultTenantStore>()
                .AddTransient<ITenantRepository, DefaultTenantRepository>()
                .AddTransient<ITenantConfigurationRepository, DefaultTenantRepository>()
                .AddTransient<ITenantResolver, TenantResolver>()
                .AddTransient<ITenantConfigurationProvider, TenantConfigurationProvider>()
                .AddTransient<ICurrentTenant, CurrentTenant>()
            );
            services.Configure<MultiTenancyCoreOptions>(action);

            return services;
        }

    }
}
