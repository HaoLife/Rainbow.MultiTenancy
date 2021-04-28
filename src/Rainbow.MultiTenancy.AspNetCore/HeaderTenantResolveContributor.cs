﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class HeaderTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "Header";

        public override string Name => ContributorName;

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request.Headers.IsNullOrEmpty())
            {
                return Task.FromResult((string)null);
            }

            var tenantIdKey = context.GetMultiTenancyCoreOptions().TenantKey;

            var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
            if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
            {
                return Task.FromResult((string)null);
            }

            if (tenantIdHeader.Count > 1)
            {
                Log(context, $"HTTP request includes more than one {tenantIdKey} header value. First one will be used. All of them: {string.Join(",", tenantIdHeader.ToArray())}");
            }

            return Task.FromResult(tenantIdHeader.First());
        }


        protected virtual void Log(ITenantResolveContext context, string text)
        {
            context
                .ServiceProvider
                .GetRequiredService<ILogger<HeaderTenantResolveContributor>>()
                .LogWarning(text);
        }
    }
}
