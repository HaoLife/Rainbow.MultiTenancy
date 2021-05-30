using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.AspNetCore;
using Rainbow.MultiTenancy.AspNetCore.Hosting;
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

            return services.AddHttpContextAccessor()
                .AddMulitTenancyCore(action);
        }

        public static IServiceCollection AddTenantService(this IServiceCollection services)
        {
            services.AddLogging();
            services.TryAddTransient<MultiTenancyServiceMiddleware>();
            services.TryAddSingleton<IEndpointRouter, EndpointRouter>();

            services.AddEndpoint<TenantListEndpoint>(TenantRoutePaths.List.EnsureLeadingSlash());
            services.AddEndpoint<TenantIdEndpoint>(TenantRoutePaths.Id.EnsureLeadingSlash());
            services.AddEndpoint<TenantNameEndpoint>(TenantRoutePaths.Name.EnsureLeadingSlash());

            return services;
        }

        public static IServiceCollection AddEndpoint<T>(this IServiceCollection services, PathString path)
            where T : class, IEndpointHandler
        {
            services.AddTransient<T>();
            services.AddSingleton(new Rainbow.MultiTenancy.AspNetCore.Hosting.Endpoint(path, typeof(T)));

            return services;
        }



        public static IServiceCollection AddTenantDefaultRemoteStores(this IServiceCollection services, Action<RemoteTenantOptions> action)
        {
            services.AddHttpClient();
            services.AddTransient<ITenantStore, MingleTenantStore>();
            services.AddTransient<ITenantService, RemoteTenantService>();
            services.AddTransient<ITenantConfigurationRepository, DefaultTenantRepository>();

            services.Configure<RemoteTenantOptions>(action);
            return services;
        }

        public static IServiceCollection AddTenantRemoteStores(this IServiceCollection services, Action<RemoteTenantOptions> action)
        {
            services.AddHttpClient();
            services.AddTransient<ITenantStore, MingleTenantStore>();
            services.AddTransient<ITenantService, RemoteTenantService>();
            services.Configure<RemoteTenantOptions>(action);

            return services;
        }
    }
}
