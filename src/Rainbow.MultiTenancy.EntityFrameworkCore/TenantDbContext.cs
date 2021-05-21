using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class TenantDbContext : DbContext
    {

        public TenantDbContext([NotNull] DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TenantModelBuilderExtensions.AddTenant(modelBuilder);
        }

    }
}
