using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.ApiAuthorization.IdentityServer
{
    public class TenantApiAuthorizationDbContext : TenantIdentityDbContext, IPersistedGrantDbContext
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public TenantApiAuthorizationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            this._operationalStoreOptions = operationalStoreOptions;
        }


        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }
}
