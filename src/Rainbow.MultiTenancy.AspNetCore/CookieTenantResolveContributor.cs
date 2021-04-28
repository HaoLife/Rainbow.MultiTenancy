using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rainbow.MultiTenancy.Core;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class CookieTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Cookie";

        public override string Name => ContributorName;

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            return Task.FromResult(httpContext.Request.Cookies[context.GetMultiTenancyCoreOptions().TenantKey]);
        }
    }
}
