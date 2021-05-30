using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rainbow.MultiTenancy.Abstractions;
using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantEntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddTenantEntityFrameworkStores<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<EfCoreTenantRepository<TContext>>();
            services.AddTransient<ITenantRepository, TenantRespositoryDecorator<EfCoreTenantRepository<TContext>>>();
            services.AddTransient<ITenantConfigurationRepository, TenantRespositoryDecorator<EfCoreTenantRepository<TContext>>>();
            services.AddTransient<ITenantStore, EfCoreTenantStore>();
            return services;
        }


        public static IServiceCollection AddTenantEntityFrameworkConfiguration<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<EfCoreTenantRepository<TContext>>();
            services.AddTransient<ITenantConfigurationRepository, TenantRespositoryDecorator<EfCoreTenantRepository<TContext>>>();
            return services;
        }
    }
}
