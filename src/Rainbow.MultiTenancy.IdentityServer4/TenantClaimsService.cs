using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Rainbow.MultiTenancy.Core;
using Rainbow.MultiTenancy.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServer4
{
    public class TenantClaimsService : DefaultClaimsService
    {
        public TenantClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }

        protected override IEnumerable<Claim> GetStandardSubjectClaims(ClaimsPrincipal subject)
        {
            var claims = base.GetStandardSubjectClaims(subject).ToList();

            claims.Add(new Claim(IdentityClaimTypes.TenantId, subject.FindTenantId()?.ToString() ?? ""));

            return claims;
        }
    }
}
