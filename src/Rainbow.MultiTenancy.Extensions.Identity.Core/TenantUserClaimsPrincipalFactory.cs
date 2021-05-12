using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantUserClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser>
        where TUser : class
    {
        public TenantUserClaimsPrincipalFactory(UserManager<TUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddTenantId(user, UserManager);
            return identity;
        }
    }

    public class TenantUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
        where TUser : class
        where TRole : class
    {
        public TenantUserClaimsPrincipalFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddTenantId(user, UserManager);
            return identity;
        }
    }
}
