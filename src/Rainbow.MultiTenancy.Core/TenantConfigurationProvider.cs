using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class TenantConfigurationProvider : ITenantConfigurationProvider
    {
        protected virtual ITenantResolver TenantResolver { get; }
        protected virtual ITenantStore TenantStore { get; }
        protected virtual ITenantResolveResultAccessor TenantResolveResultAccessor { get; }

        public TenantConfigurationProvider(
            ITenantResolver tenantResolver,
            ITenantStore tenantStore,
            ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            TenantResolver = tenantResolver;
            TenantStore = tenantStore;
            TenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        public async Task<TenantConfiguration> GetAsync(bool saveResolveResult = false)
        {
            var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

            if (saveResolveResult)
            {
                TenantResolveResultAccessor.Result = resolveResult;
            }

            TenantConfiguration tenant = null;
            if (resolveResult.TenantIdOrName != null)
            {
                tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

                if (tenant == null)
                {
                    throw new Exception(
                        message: "Tenant not found!"
                    );
                }
            }

            return tenant;
        }


        protected virtual async Task<TenantConfiguration> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await TenantStore.FindAsync(parsedTenantId);
            }
            else
            {
                return await TenantStore.FindAsync(tenantIdOrName);
            }
        }
    }
}
