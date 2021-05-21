using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class EfCoreTenantRepository<TContext> : ITenantRepository
        , ITenantConfigurationRepository
        where TContext : DbContext
    {
        private readonly TContext context;

        public EfCoreTenantRepository(TContext context)
        {
            this.context = context;
        }

        public Task<Tenant> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.context.Set<Tenant>()
                .Include(a => a.ConfigurationStrings)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task<Tenant> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return this.context.Set<Tenant>()
                .Include(a => a.ConfigurationStrings)
                .FirstOrDefaultAsync(a => a.Name == name);
        }


        public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return this.context.Set<Tenant>()
                .LongCountAsync();
        }

        public Task<List<Tenant>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return this.context.Set<Tenant>()
                .Include(a => a.ConfigurationStrings)
                .ToListAsync();
        }

        public Task<List<TenantConfigurationString>> FindByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return this.context.Set<TenantConfigurationString>()
                .Where(a => a.TenantId == tenantId)
                .ToListAsync();
        }
    }
}
