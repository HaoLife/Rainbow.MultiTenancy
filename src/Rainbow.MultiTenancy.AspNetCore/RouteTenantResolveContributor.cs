using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class RouteTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Route";

        public override string Name => ContributorName;

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            var tenantId = httpContext.GetRouteValue(context.GetMultiTenancyCoreOptions().TenantKey);
            return Task.FromResult(tenantId != null ? Convert.ToString(tenantId) : null);
        }
    }
}
