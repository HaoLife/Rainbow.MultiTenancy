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
            services.AddScoped<ITenantRepository, EfCoreTenantRepository<TContext>>();
            services.AddScoped<ITenantConfigurationRepository, EfCoreTenantRepository<TContext>>();
            services.AddScoped<ITenantStore, EfCoreTenantStore>();
            return services;
        }
    }
}
