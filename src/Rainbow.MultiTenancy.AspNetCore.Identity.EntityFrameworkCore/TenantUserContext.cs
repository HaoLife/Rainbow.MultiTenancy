using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore
{
    public abstract class TenantUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : DbContext
        where TUser : TenantUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : TenantUserClaim<TKey>
        where TUserLogin : TenantUserLogin<TKey>
        where TUserToken : TenantUserToken<TKey>
    {

        public TenantUserContext(DbContextOptions options) : base(options) { }

        protected TenantUserContext() { }

        public virtual DbSet<TUser> Users { get; set; }

        public virtual DbSet<TUserClaim> UserClaims { get; set; }

        public virtual DbSet<TUserLogin> UserLogins { get; set; }

        public virtual DbSet<TUserToken> UserTokens { get; set; }

        private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ModelBuilderExtensions.AddTenantUser<TUser, TKey, TUserClaim, TUserLogin, TUserToken>(builder, this);

        }
    }
}
