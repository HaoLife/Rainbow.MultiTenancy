using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore
{
    public class TenantRoleStore<TRole, TContext, TKey>
        : TenantRoleStore<TRole, TContext, TKey, TenantUserRole<TKey>, TenantRoleClaim<TKey>>,
        IQueryableRoleStore<TRole>,
        IRoleClaimStore<TRole>
        where TRole : TenantRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        /// <summary>
        /// Constructs a new instance of <see cref="RoleStore{TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public TenantRoleStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    public class TenantRoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        : RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        , ITenantHandlerStore<TRole>
        , ITenantRoleQueryStore<TRole>
        where TRole : TenantRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
        where TUserRole : TenantUserRole<TKey>, new()
        where TRoleClaim : TenantRoleClaim<TKey>, new()
    {
        public TenantRoleStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {

        }

        public virtual Task<Guid?> GetTanantAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.TenantId);
        }

        public virtual Task SetTenantAsync(TRole role, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.TenantId = tenantId;
            return Task.CompletedTask;
        }


        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            return this.FindByNameAsync(normalizedName, null, cancellationToken);
        }

        public override Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.FindByIdAsync(id, null, cancellationToken);
        }

        public Task<TRole> FindByIdAsync(string id, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId) && u.TenantId == tenantId, cancellationToken);
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName && r.TenantId == tenantId, cancellationToken);
        }
    }
}
