using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class MultiTenancyConnectionInterceptor : DbConnectionInterceptor
    {
        private readonly IServiceProvider provider;

        public MultiTenancyConnectionInterceptor(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            var currentTenant = provider.GetRequiredService<ICurrentTenant>();
            if (!currentTenant.IsAvailable) return base.ConnectionOpening(connection, eventData, result);

            var tenantStore = provider.GetRequiredService<ITenantStore>();
            var tenantConfiguration = tenantStore.FindAsync(currentTenant.Id.Value).GetAwaiter().GetResult();

            var conn = tenantConfiguration.ConnectionStrings.Default;

            eventData.Context.Database.SetConnectionString(conn);
            return result;
        }
    }
}
