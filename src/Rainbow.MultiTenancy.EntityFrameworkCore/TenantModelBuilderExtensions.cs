using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public static class TenantModelBuilderExtensions
    {

        public static ModelBuilder AddTenant(this ModelBuilder builder)
        {

            builder.Entity<Tenant>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.Name).HasDatabaseName("NameIndex").IsUnique();
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.CreatorId).HasMaxLength(64);
                b.Property(u => u.LastUpdatorId).HasMaxLength(64);

                b.HasMany(a => a.ConfigurationStrings).WithOne().HasForeignKey(a => a.TenantId);
                //b.HasMany<TenantConfigurationString>().WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();
            });

            builder.AddOnlyTenantConfiguration();

            return builder;
        }

        public static ModelBuilder AddOnlyTenantConfiguration(this ModelBuilder builder)
        {

            builder.Entity<TenantConfigurationString>(b =>
            {
                b.HasKey(u => new { u.TenantId, u.Name });
                b.Property(u => u.Value).IsRequired();

            });

            return builder;
        }
    }
}
