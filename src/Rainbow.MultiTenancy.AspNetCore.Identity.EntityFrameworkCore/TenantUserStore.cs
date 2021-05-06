using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
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
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TRole, TContext, TKey}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public TenantUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }
    public class TenantUserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        , ITenantHandlerStore<TUser>
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


        public virtual Task SetTenant(TUser user, Guid? tenantId, CancellationToken cancellationToken)
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
