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
        public TenantUserOnlyStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    public class TenantUserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
        : UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>
         , ITenantHandlerStore<TUser>
        where TUser : TenantUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : TenantUserClaim<TKey>, new()
        where TUserLogin : TenantUserLogin<TKey>, new()
        where TUserToken : TenantUserToken<TKey>, new()
    {
        public TenantUserOnlyStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public Task SetTenant(TUser user, Guid? tenantId, CancellationToken cancellationToken)
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
