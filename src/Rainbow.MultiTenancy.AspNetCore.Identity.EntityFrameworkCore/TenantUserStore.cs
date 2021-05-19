using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore
{
    public class TenantUserStore<TUser, TRole, TContext, TKey>
        : TenantUserStore<TUser, TRole, TContext, TKey, TenantUserClaim<TKey>, TenantUserRole<TKey>, TenantUserLogin<TKey>, TenantUserToken<TKey>, TenantRoleClaim<TKey>>
        where TUser : TenantUser<TKey>
        where TRole : TenantRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public TenantUserStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null) : base(currentTenant, context, describer) { }
    }
    public class TenantUserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        where TUser : TenantUser<TKey>
        where TRole : TenantRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : TenantUserClaim<TKey>, new()
        where TUserRole : TenantUserRole<TKey>, new()
        where TUserLogin : TenantUserLogin<TKey>, new()
        where TUserToken : TenantUserToken<TKey>, new()
        where TRoleClaim : TenantRoleClaim<TKey>, new()

    {
        private readonly ICurrentTenant currentTenant;

        public TenantUserStore(ICurrentTenant currentTenant, TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
            this.currentTenant = currentTenant;
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
