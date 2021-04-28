using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class MultiTenancyMiddleware : IMiddleware
    {
        private readonly ITenantConfigurationProvider _tenantConfigurationProvider;
        private readonly ICurrentTenant _currentTenant;

        public MultiTenancyMiddleware(
            ITenantConfigurationProvider tenantConfigurationProvider,
            ICurrentTenant currentTenant)
        {
            _tenantConfigurationProvider = tenantConfigurationProvider;
            _currentTenant = currentTenant;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            var tenant = await _tenantConfigurationProvider.GetAsync(saveResolveResult: true);
            if (tenant?.Id != _currentTenant.Id)
            {
                using (_currentTenant.Change(tenant?.Id, tenant?.Name))
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
