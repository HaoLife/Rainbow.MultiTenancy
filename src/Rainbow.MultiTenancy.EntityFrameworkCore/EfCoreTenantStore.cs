using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class EfCoreTenantStore : ITenantStore
    {
        private readonly ITenantRepository tenantRepository;

        public EfCoreTenantStore(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        public async Task<TenantConfiguration> FindAsync(string name)
        {
            var model = await this.tenantRepository.FindByNameAsync(name);

            return this.Map(model);
        }

        public async Task<TenantConfiguration> FindAsync(Guid id)
        {
            var model = await this.tenantRepository.FindByIdAsync(id);

            return this.Map(model);
        }

        public TenantConfiguration Map(Tenant model)
        {
            if (model == null) return null;
            var connectionStrings = new ConnectionStrings();
            model.ConfigurationStrings.ForEach(a => connectionStrings[a.Name] = a.Value);
            return new TenantConfiguration()
            {
                Id = model.Id,
                Name = model.Name,
                ConnectionStrings = connectionStrings,
            };
        }

    }
}
