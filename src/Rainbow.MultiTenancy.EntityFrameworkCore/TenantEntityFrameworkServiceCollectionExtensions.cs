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
            services.AddScoped<ITenantRepository, TenantRespositoryDecorator<EfCoreTenantRepository<TContext>>>();
            services.AddScoped<ITenantConfigurationRepository, TenantRespositoryDecorator<EfCoreTenantRepository<TContext>>>();
            services.AddScoped<ITenantStore, EfCoreTenantStore>();
            return services;
        }
    }
}
