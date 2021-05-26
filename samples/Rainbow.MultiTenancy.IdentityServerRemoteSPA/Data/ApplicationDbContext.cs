using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.AspNetCore.ApiAuthorization.IdentityServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.IdentityServerRemoteSPA.Data
{
    public class ApplicationDbContext : TenantApiAuthorizationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            EntityFrameworkCore.TenantModelBuilderExtensions.AddTenant(builder);
        }
    }
}
