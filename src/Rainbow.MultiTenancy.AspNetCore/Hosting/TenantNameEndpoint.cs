using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class TenantNameEndpoint : BaseEndpoint
    {
        private readonly ITenantRepository tenantRepository;

        public TenantNameEndpoint(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        public override async Task<IEndpointResult> ProcessContentAsync(HttpContext context)
        {
            var key = "name";

            if (!context.Request.Query.ContainsKey(key) && !string.IsNullOrEmpty(context.Request.Query[key]))
            {
                return new ParameterErrorResponse(key);
            }

            string name = context.Request.Query[key];

            var result = await this.tenantRepository.FindByNameAsync(name);

            var content = result == null ? null :
                new TenantDto { Id = result.Id, Name = result.Name };


            return new ContentResult(content);
        }

    }
}
