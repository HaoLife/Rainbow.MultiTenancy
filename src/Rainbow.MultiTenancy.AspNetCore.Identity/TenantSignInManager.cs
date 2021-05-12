using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Identity
{
    public class TenantSignInManager<TUser> : SignInManager<TUser>
        where TUser : class
    {
        private readonly TenantUserManager<TUser> tenantUserManager;

        public TenantSignInManager(TenantUserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            this.tenantUserManager = userManager;
        }

        public virtual async Task<SignInResult> PasswordSignInAsync(TUser user, string password,
            Guid? tenantId, bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var attempt = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return attempt.Succeeded
                ? await SignInOrTwoFactorAsync(user, isPersistent)
                : attempt;
        }

        public virtual async Task<SignInResult> PasswordSignInAsync(string userName, string password,
           Guid? tenantId, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await tenantUserManager.FindByNameAsync(userName, tenantId);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await PasswordSignInAsync(user, password, tenantId, isPersistent, lockoutOnFailure);
        }

    }
}
