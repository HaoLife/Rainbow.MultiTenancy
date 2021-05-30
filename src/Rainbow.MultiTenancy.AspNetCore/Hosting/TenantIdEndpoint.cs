using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class TenantIdEndpoint : BaseEndpoint
    {
        private readonly ITenantRepository tenantRepository;

        public TenantIdEndpoint(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        public override async Task<IEndpointResult> ProcessContentAsync(HttpContext context)
        {
            var key = "id";
            Guid id;


            if (!context.Request.Query.ContainsKey(key) || !Guid.TryParse(context.Request.Query[key], out id))
            {
                return new ParameterErrorResponse(key);
            }

            var result = await this.tenantRepository.FindByIdAsync(id);

            var content = result == null ? null :
                new TenantDto { Id = result.Id, Name = result.Name };


            return new ContentResult(content);
        }


    }
}
