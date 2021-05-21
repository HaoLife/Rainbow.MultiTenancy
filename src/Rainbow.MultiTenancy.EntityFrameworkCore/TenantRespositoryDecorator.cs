using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class TenantRespositoryDecorator<TTenantRespository> : ITenantRepository
        , ITenantConfigurationRepository
        where TTenantRespository : ITenantRepository, ITenantConfigurationRepository
    {
        private readonly TTenantRespository tenantRespository;
        private readonly ICurrentTenant currentTenant;

        public TenantRespositoryDecorator(TTenantRespository tenantRespository, ICurrentTenant currentTenant)
        {
            this.tenantRespository = tenantRespository;
            this.currentTenant = currentTenant;
        }

        public Task<Tenant> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return this.Handle(() => tenantRespository.FindByIdAsync(id, cancellationToken));
        }

        public Task<Tenant> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return this.Handle(() => tenantRespository.FindByNameAsync(name, cancellationToken));
        }

        public Task<List<TenantConfigurationString>> FindByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return this.Handle(() => tenantRespository.FindByTenantIdAsync(tenantId, cancellationToken));
        }

        public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return this.Handle(() => tenantRespository.GetCountAsync( cancellationToken));
        }

        public Task<List<Tenant>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return this.Handle(() => tenantRespository.GetListAsync(includeDetails, cancellationToken));
        }


        protected T Handle<T>(Func<T> func)
        {
            using (this.currentTenant.Change(null))
                return func();
        }
    }
}
