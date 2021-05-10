using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantRoleManager<TRole> : RoleManager<TRole> where TRole : class
    {
        public TenantRoleManager(IRoleStore<TRole> store, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }


        public virtual bool SupportsTenant
        {
            get
            {
                ThrowIfDisposed();
                return Store is ITenantHandlerStore<TRole> && Store is ITenantRoleQueryStore<TRole>;
            }
        }

        public virtual Task<TRole> FindByNameAsync(string roleName, Guid? tenantId)
        {
            ThrowIfDisposed();

            if (!this.SupportsTenant)
            {
                throw new NotSupportedException();
            }

            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }
            var query = Store as ITenantRoleQueryStore<TRole>;

            return query.FindByNameAsync(NormalizeKey(roleName), tenantId, CancellationToken);
        }

        public virtual async Task<Guid?> GetTanantIdAsync(TRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (!this.SupportsTenant)
            {
                throw new NotSupportedException();
            }

            var handler = this.Store as ITenantHandlerStore<TRole>;

            return await handler.GetTanantAsync(role, CancellationToken);
        }


        

    }
}
