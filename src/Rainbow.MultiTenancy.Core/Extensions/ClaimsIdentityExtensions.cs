using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static Guid? FindUserId(this ClaimsPrincipal principal)
        {
            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == IdentityClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(userIdOrNull.Value, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static Guid? FindTenantId(this ClaimsPrincipal principal)
        {
            var tenantIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == IdentityClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(tenantIdOrNull.Value);
        }
    }
}
