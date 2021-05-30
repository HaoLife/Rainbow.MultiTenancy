using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class TenantListEndpoint : BaseEndpoint
    {
        private readonly ITenantRepository tenantRepository;

        public TenantListEndpoint(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }
        public override async Task<IEndpointResult> ProcessContentAsync(HttpContext context)
        {
            var result = await this.tenantRepository.GetListAsync();

            var content = result.Select(a => new TenantDto() { Id = a.Id, Name = a.Name }).ToList();

            return new ContentResult(content);
        }
    }
}
