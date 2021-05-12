using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.AspNetCore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class DomainTenantResolveContributor : HttpTenantResolveContributorBase
    {

        public const string ContributorName = "Domain";

        public override string Name => ContributorName;

        private static readonly string[] ProtocolPrefixes = { "http://", "https://" };

        private readonly string _domainFormat;

        public DomainTenantResolveContributor(string domainFormat)
        {
            _domainFormat = this.RemovePre(domainFormat, ProtocolPrefixes);
        }

        protected override Task<string> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.Host.HasValue)
            {
                return Task.FromResult<string>(null);
            }

            var hostName = this.RemovePre(httpContext.Request.Host.Value, ProtocolPrefixes);
            var extractResult = FormattedStringValueExtracter.Extract(hostName, _domainFormat, ignoreCase: true);

            context.Handled = true;

            return Task.FromResult(extractResult.IsMatch ? extractResult.Matches[0].Value : null);
        }


        public string RemovePre(string domain, string[] pres)
        {
            foreach (var item in pres)
            {
                if (!domain.StartsWith(item, StringComparison.OrdinalIgnoreCase)) continue;
                domain = domain.Substring(item.Length);
            }
            return domain;
        }

    }
}
