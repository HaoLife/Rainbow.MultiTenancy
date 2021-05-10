using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityServiceCollectionExtensions
    {

        public static TenantIdentityBuilder AddTenantIdentityCore<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction) where TUser : class
        {

            services.AddOptions().AddLogging();
            services.TryAddScoped<IUserValidator<TUser>, TenantUserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
            services.TryAddScoped<UserManager<TUser>, TenantUserManager<TUser>>();
            services.TryAddScoped<TenantUserManager<TUser>>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new TenantIdentityBuilder(typeof(TUser), services);
        }
    }
}
