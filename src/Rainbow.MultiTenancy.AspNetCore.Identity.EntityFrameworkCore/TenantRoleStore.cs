using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Abstractions;
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
        public TenantRoleStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null)
            : base(currentTenant, context, describer) { }
    }

    public class TenantRoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        : RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>
        where TRole : TenantRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
        where TUserRole : TenantUserRole<TKey>, new()
        where TRoleClaim : TenantRoleClaim<TKey>, new()
    {
        private readonly ICurrentTenant currentTenant;

        public TenantRoleStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
            this.currentTenant = currentTenant;
        }

        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName && r.TenantId == this.currentTenant.Id, cancellationToken);

        }

        public override Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var roleId = ConvertIdFromString(id);
            return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId) && u.TenantId == this.currentTenant.Id, cancellationToken);
        }

    }
}
