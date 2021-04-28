using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class QueryStringTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "QueryString";

        public override string Name => ContributorName;

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            return Task.FromResult(httpContext.Request.QueryString.HasValue
                ? httpContext.Request.Query[context.GetMultiTenancyCoreOptions().TenantKey].ToString()
                : null);
        }
    }
}
