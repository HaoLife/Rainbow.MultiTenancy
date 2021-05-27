using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServer4.AspNetIdentity
{
    public class TenantProfileService<TUser> : ProfileService<TUser>
        where TUser : class
    {
        private readonly ICurrentTenant currentTenant;

        public TenantProfileService(ICurrentTenant currentTenant, UserManager<TUser> userManager, IUserClaimsPrincipalFactory<TUser> claimsFactory)
            : base(userManager, claimsFactory)
        {
            this.currentTenant = currentTenant;
        }

        public TenantProfileService(ICurrentTenant currentTenant, UserManager<TUser> userManager, IUserClaimsPrincipalFactory<TUser> claimsFactory, ILogger<TenantProfileService<TUser>> logger)
            : base(userManager, claimsFactory, logger)
        {
            this.currentTenant = currentTenant;
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (this.currentTenant.Change(context.Subject.FindTenantId()))
                return base.GetProfileDataAsync(context);
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            using (this.currentTenant.Change(context.Subject.FindTenantId()))
                return base.IsActiveAsync(context);
        }

    }
}
