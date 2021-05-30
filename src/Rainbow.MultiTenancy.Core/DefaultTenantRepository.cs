using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Rainbow.MultiTenancy.Core
{
    public class DefaultTenantRepository : ITenantRepository, ITenantConfigurationRepository
    {
        private readonly MultiTenancyCoreOptions _options;

        public DefaultTenantRepository(IOptionsSnapshot<MultiTenancyCoreOptions> options)
        {
            _options = options.Value;
        }

        public List<TenantConfigurationString> Map(Guid tenantId, ConnectionStrings strings)
        {
            var list = new List<TenantConfigurationString>();
            foreach (var key in strings.AllKeys)
            {
                list.Add(new TenantConfigurationString()
                {
                    Name = key,
                    Value = strings[key],
                    TenantId = tenantId,
                });
            }

            return list;
        }

        public Tenant Map(TenantConfiguration tenant)
        {
            if (tenant == null) return null;

            return new Tenant()
            {
                Id = tenant.Id,
                Name = tenant.Name,
                CreateTime = DateTime.Now,
                ConfigurationStrings = Map(tenant.Id, tenant.ConnectionStrings),
            };
        }

        public Task<Tenant> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var tenant = this._options.Tenants.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(Map(tenant));
        }

        public Task<Tenant> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var tenant = this._options.Tenants.FirstOrDefault(a => a.Name == name);
            return Task.FromResult(Map(tenant));
        }

        public Task<List<TenantConfigurationString>> FindByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            var tenant = this._options.Tenants.FirstOrDefault(a => a.Id == tenantId);

            return Task.FromResult(Map(tenantId, tenant.ConnectionStrings));
        }

        public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this._options.Tenants.LongCount());
        }

        public Task<List<Tenant>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var list = this._options.Tenants.Select(a => this.Map(a)).ToList();

            return Task.FromResult(list);
        }
    }
}
