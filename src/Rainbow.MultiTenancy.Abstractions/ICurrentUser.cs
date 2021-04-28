using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid? Id { get; }
        string UserName { get; }
        string Name { get; }

        string SurName { get; }

        string PhoneNumber { get; }

        bool PhoneNumberVerified { get; }

        string Email { get; }

        bool EmailVerified { get; }

        Guid? TenantId { get; }
        string[] Roles { get; }
        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);
        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
