using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantRoleValidator<TRole> : IRoleValidator<TRole> where TRole : class
    {
        private IdentityErrorDescriber Describer { get; set; }

        public TenantRoleValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }


        /// <summary>
        /// Validates a role as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="RoleManager{TRole}"/> managing the role store.</param>
        /// <param name="role">The role to validate.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous validation.</returns>
        public virtual async Task<IdentityResult> ValidateAsync(RoleManager<TRole> manager, TRole role)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            var errors = new List<IdentityError>();
            await ValidateRoleName(manager as TenantRoleManager<TRole>, role, errors);
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task ValidateRoleName(TenantRoleManager<TRole> manager, TRole role,
            ICollection<IdentityError> errors)
        {
            var roleName = await manager.GetRoleNameAsync(role);
            Guid? tenantId = await manager.GetTanantIdAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                errors.Add(Describer.InvalidRoleName(roleName));
            }
            else
            {
                var owner = await manager.FindByNameAsync(roleName, tenantId);
                if (owner != null &&
                    !string.Equals(await manager.GetRoleIdAsync(owner), await manager.GetRoleIdAsync(role)))
                {
                    errors.Add(Describer.DuplicateRoleName(roleName));
                }
            }
        }
    }
}
