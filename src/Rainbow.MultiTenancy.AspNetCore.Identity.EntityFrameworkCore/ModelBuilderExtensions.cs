using Microsoft.EntityFrameworkCore;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddTenantUser<TUser, TKey, TUserClaim, TUserLogin, TUserToken>(this ModelBuilder builder
            , DbContext dbContext)
            where TKey : IEquatable<TKey>
            where TUser : TenantUser<TKey>
            where TUserClaim : TenantUserClaim<TKey>
            where TUserLogin : TenantUserLogin<TKey>
            where TUserToken : TenantUserToken<TKey>
        {
            var storeOptions = dbContext.GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;

            int maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            PersonalDataConverter converter = null;

            builder.Entity<TUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => new { u.NormalizedUserName, u.TenantId }).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => new { u.NormalizedEmail, u.TenantId }).HasDatabaseName("EmailIndex");
                b.ToTable($"{nameof(TenantUser)}s");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                if (encryptPersonalData)
                {
                    var personalDataProtector = dbContext.GetService<IPersonalDataProtector>();
                    converter = new PersonalDataConverter(personalDataProtector);
                    var personalDataProps = typeof(TUser).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in personalDataProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }

                b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            });

            builder.Entity<TUserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable($"{nameof(TenantUserClaim)}s");
            });

            builder.Entity<TUserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.TenantId });

                if (maxKeyLength > 0)
                {
                    b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
                }

                b.ToTable($"{nameof(TenantUserLogin)}s");
            });

            builder.Entity<TUserToken>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name, t.TenantId });

                if (maxKeyLength > 0)
                {
                    b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(t => t.Name).HasMaxLength(maxKeyLength);
                }

                if (encryptPersonalData)
                {
                    var tokenProps = typeof(TUserToken).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in tokenProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }

                b.ToTable($"{nameof(TenantUserToken)}s");
            });

            return builder;
        }


        public static ModelBuilder AddTenantRole<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>(this ModelBuilder builder
            , DbContext dbContext)
            where TUser : TenantUser<TKey>
            where TRole : TenantRole<TKey>
            where TKey : IEquatable<TKey>
            where TUserClaim : TenantUserClaim<TKey>
            where TUserRole : TenantUserRole<TKey>
            where TUserLogin : TenantUserLogin<TKey>
            where TRoleClaim : TenantRoleClaim<TKey>
            where TUserToken : TenantUserToken<TKey>
        {
            builder.Entity<TUser>(b =>
            {
                b.HasMany<TUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<TRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => new { r.NormalizedName, r.TenantId }).HasDatabaseName("RoleNameIndex").IsUnique();
                b.ToTable($"{nameof(TenantRole)}s");
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                b.HasMany<TUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany<TRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<TRoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable($"{nameof(TenantRoleClaim)}s");
            });

            builder.Entity<TUserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId, r.TenantId });
                b.ToTable($"{nameof(TenantUserRole)}s");
            });

            return builder;
        }

    }
}
