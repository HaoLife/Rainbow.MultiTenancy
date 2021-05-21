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
    public class TenantUserOnlyStore<TUser, TContext, TKey>
        : TenantUserOnlyStore<TUser, TContext, TKey, TenantUserClaim<TKey>, TenantUserLogin<TKey>, TenantUserToken<TKey>>
       where TUser : TenantUser<TKey>
       where TContext : DbContext
       where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public TenantUserOnlyStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null) : base(currentTenant, context, describer) { }
    }

    public class TenantUserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
        : UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : TenantUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : TenantUserClaim<TKey>, new()
        where TUserLogin : TenantUserLogin<TKey>, new()
        where TUserToken : TenantUserToken<TKey>, new()
    {

        private readonly ICurrentTenant currentTenant;

        public TenantUserOnlyStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
            this.currentTenant = currentTenant;
        }

        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            user.TenantId = this.currentTenant.Id;
            return base.CreateAsync(user, cancellationToken);
        }
        protected override async Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return await this.Context.Set<TUserToken>()
                .FirstOrDefaultAsync(a => a.UserId.Equals(user.Id) && a.LoginProvider == loginProvider && a.TenantId == this.currentTenant.Id);

        }
        public override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            return this.Users.FirstOrDefaultAsync(a => a.Id.Equals(id) && a.TenantId == this.currentTenant.Id, cancellationToken);
        }
        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName && u.TenantId == this.currentTenant.Id, cancellationToken);

        }
        public override async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            var tenantId = this.currentTenant.Id;
            var login = await this.Context.Set<TUserLogin>().FirstOrDefaultAsync(a => a.LoginProvider == loginProvider && a.ProviderKey == providerKey && a.TenantId == tenantId);
            if (login != null)
            {
                return await this.Users.FirstOrDefaultAsync(a => a.Id.Equals(login.UserId) && a.TenantId == tenantId);
            }
            return null;
        }
        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.TenantId == this.currentTenant.Id, cancellationToken);

        }

    }
}
