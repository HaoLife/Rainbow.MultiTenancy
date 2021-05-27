using IdentityModel;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServer4
{
    public static class ClaimsIdentityServer4Extensions
    {
        public static Guid? FindIdsUserId(this ClaimsPrincipal principal)
        {
            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
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

    }
}
