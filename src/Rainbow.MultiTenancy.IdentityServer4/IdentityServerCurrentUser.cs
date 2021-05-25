using IdentityModel;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServer4
{
    public class IdentityServerCurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        public virtual bool IsAuthenticated => Id.HasValue;

        public virtual Guid? Id => _principalAccessor.Principal?.FindUserId();

        public virtual string UserName => this.FindClaimValue(JwtClaimTypes.GivenName);

        public virtual string Name => this.FindClaimValue(JwtClaimTypes.Name);

        public virtual string SurName => this.FindClaimValue(JwtClaimTypes.Name);

        public virtual string PhoneNumber => this.FindClaimValue(JwtClaimTypes.PhoneNumber);

        public virtual bool PhoneNumberVerified => string.Equals(this.FindClaimValue(JwtClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual string Email => this.FindClaimValue(JwtClaimTypes.Email);

        public virtual bool EmailVerified => string.Equals(this.FindClaimValue(JwtClaimTypes.EmailVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public virtual Guid? TenantId => _principalAccessor.Principal?.FindTenantId();

        public virtual string[] Roles => FindClaims(JwtClaimTypes.Role).Select(c => c.Value).ToArray();

        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public IdentityServerCurrentUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        public virtual Claim[] GetAllClaims()
        {
            return _principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;
        }

        public virtual bool IsInRole(string roleName)
        {
            return FindClaims(IdentityClaimTypes.Role).Any(c => c.Value == roleName);
        }
    }
}
