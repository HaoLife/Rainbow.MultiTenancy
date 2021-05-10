using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantUserValidator<TUser> : UserValidator<TUser> where TUser : class
    {
        public TenantUserValidator(IdentityErrorDescriber errors = null)
            : base(errors)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (!(manager is TenantUserManager<TUser>))
            {
                return base.ValidateAsync(manager, user).GetAwaiter().GetResult();
            }


            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = new List<IdentityError>();
            await ValidateUserName((TenantUserManager<TUser>)manager, user, errors);
            if (manager.Options.User.RequireUniqueEmail)
            {
                await ValidateEmail((TenantUserManager<TUser>)manager, user, errors);
            }
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }


        private async Task ValidateUserName(TenantUserManager<TUser> manager, TUser user, ICollection<IdentityError> errors)
        {
            var userName = await manager.GetUserNameAsync(user);
            var tenantId = await manager.GetTanantIdAsync(user);
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
            {
                errors.Add(Describer.InvalidUserName(userName));
            }
            else
            {
                var owner = await manager.FindByNameAsync(userName, tenantId);
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(Describer.DuplicateUserName(userName));
                }
            }
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(TenantUserManager<TUser> manager, TUser user, List<IdentityError> errors)
        {
            var email = await manager.GetEmailAsync(user);
            var tenantId = await manager.GetTanantIdAsync(user);
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(Describer.InvalidEmail(email));
                return;
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                errors.Add(Describer.InvalidEmail(email));
                return;
            }
            var owner = await manager.FindByEmailAsync(email);
            if (owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
            {
                errors.Add(Describer.DuplicateEmail(email));
            }
        }

    }
}
