using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore
{
    public class TenantIdentityDbContext
        : TenantIdentityDbContext<TenantUser, TenantRole, string>
    {
        public TenantIdentityDbContext(DbContextOptions options) : base(options) { }

        protected TenantIdentityDbContext() { }
    }

    public class TenantIdentityDbContext<TUser, TRole, TKey>
        : TenantIdentityDbContext<TUser, TRole, TKey, TenantUserClaim<TKey>, TenantUserRole<TKey>, TenantUserLogin<TKey>, TenantRoleClaim<TKey>, TenantUserToken<TKey>>
        where TUser : TenantUser<TKey>
        where TRole : TenantRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the db context.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public TenantIdentityDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected TenantIdentityDbContext() { }
    }

    public abstract class TenantIdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : TenantUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : TenantUser<TKey>
        where TRole : TenantRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : TenantUserClaim<TKey>
        where TUserRole : TenantUserRole<TKey>
        where TUserLogin : TenantUserLogin<TKey>
        where TRoleClaim : TenantRoleClaim<TKey>
        where TUserToken : TenantUserToken<TKey>
    {

        public TenantIdentityDbContext(DbContextOptions options) : base(options) { }

        protected TenantIdentityDbContext() { }

        public virtual DbSet<TUserRole> UserRoles { get; set; }

        public virtual DbSet<TRole> Roles { get; set; }

        public virtual DbSet<TRoleClaim> RoleClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            TenantIdentityModelBuilderExtensions.AddTenantRole<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>(builder, this);
        }
    }
}
