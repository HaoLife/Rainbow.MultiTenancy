using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public TenantUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }
    public class TenantUserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        , ITenantHandlerStore<TUser>
        , ITenantUserQueryStore<TUser>
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
        public TenantUserStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {

        }

        protected override async Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            return await this.Context.Set<TUserToken>()
                .FirstOrDefaultAsync(a => a.UserId.Equals(user.Id) && a.LoginProvider == loginProvider && a.TenantId == user.TenantId);

        }
        public override Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return this.FindByIdAsync(userId, null, cancellationToken);
        }
        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            return this.FindByNameAsync(normalizedUserName, null, cancellationToken);
        }
        public override Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            return this.FindByLoginAsync(loginProvider, providerKey, null, cancellationToken);
        }
        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            return this.FindByEmailAsync(normalizedEmail, null, cancellationToken);
        }

        public virtual Task<TUser> FindByIdAsync(string userId, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            return this.Users.FirstOrDefaultAsync(a => a.Id.Equals(id) && a.TenantId == tenantId, cancellationToken);
        }

        public virtual Task<TUser> FindByNameAsync(string normalizedUserName, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName && u.TenantId == tenantId, cancellationToken);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail && u.TenantId == tenantId, cancellationToken);

        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, Guid? tenantId, CancellationToken cancellationToken = default)
        {
            var login = await this.Context.Set<TUserLogin>().FirstOrDefaultAsync(a => a.LoginProvider == loginProvider && a.ProviderKey == providerKey && tenantId == tenantId);
            if (login != null)
            {
                return await this.Users.FirstOrDefaultAsync(a => a.Id.Equals(login.UserId) && a.TenantId == tenantId);
            }
            return null;
        }

        public virtual Task<Guid?> GetTanantAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.TenantId);
        }

        public virtual Task SetTenantAsync(TUser user, Guid? tenantId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.TenantId = tenantId;
            return Task.CompletedTask;
        }

    }
}
