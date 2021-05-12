﻿using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public static class ClaimsIdentityExtensions
    {
        public static ClaimsIdentity AddTenantId<TUser>(this ClaimsIdentity identity, TUser user, UserManager<TUser> userManager)
            where TUser : class
        {
            var tenantUserManager = userManager as TenantUserManager<TUser>;
            if (tenantUserManager != null)
            {
                var tenantId = tenantUserManager.GetTanantIdAsync(user).GetAwaiter().GetResult();
                if (tenantId.HasValue)
                    identity.AddClaim(new Claim(IdentityClaimTypes.TenantId, tenantId.ToString()));
            }
            return identity;
        }
    }
}
