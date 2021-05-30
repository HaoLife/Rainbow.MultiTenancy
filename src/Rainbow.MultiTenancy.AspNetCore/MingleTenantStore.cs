using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class MingleTenantStore : ITenantStore
    {
        private readonly ITenantService tenantService;
        private readonly ITenantConfigurationRepository tenantConfigurationRepository;

        public MingleTenantStore(ITenantService tenantService, ITenantConfigurationRepository tenantConfigurationRepository)
        {
            this.tenantService = tenantService;
            this.tenantConfigurationRepository = tenantConfigurationRepository;
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            var tenant = await this.tenantService.GetAsync(name);
            if (tenant == null) return null;

            var configs = await tenantConfigurationRepository.FindByTenantIdAsync(tenant.Id);

            return Map(tenant, configs);
        }

        public async Task<TenantConfiguration> FindAsync(Guid id)
        {
            var tenant = await this.tenantService.GetAsync(id);
            if (tenant == null) return null;

            var configs = await tenantConfigurationRepository.FindByTenantIdAsync(tenant.Id);

            return Map(tenant, configs);
        }

        private TenantConfiguration Map(TenantDto tenant, List<TenantConfigurationString> configs)
        {
            if (tenant == null) return null;
            ConnectionStrings strings = new ConnectionStrings();
            if (configs != null)
            {
                foreach (var item in configs)
                    strings[item.Name] = item.Value;
            }
            return new TenantConfiguration()
            {
                Id = tenant.Id,
                Name = tenant.Name,
                ConnectionStrings = strings,
            };
        }
    }
}
