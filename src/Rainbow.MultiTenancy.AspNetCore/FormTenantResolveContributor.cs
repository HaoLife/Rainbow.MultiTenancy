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
    public class FormTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Form";

        public override string Name => ContributorName;

        protected override async Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.HasFormContentType)
            {
                return null;
            }

            var form = await httpContext.Request.ReadFormAsync();
            return form[context.GetMultiTenancyCoreOptions().TenantKey];
        }
    }
}
