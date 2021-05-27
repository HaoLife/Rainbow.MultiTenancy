using Rainbow.MultiTenancy.IdentityServer4.AspNetIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddTenantAspNetIdentityServer<TUser>(this IIdentityServerBuilder builder)
            where TUser : class
        {
            builder.AddProfileService<TenantProfileService<TUser>>();
            
            return builder;
        }
    }
}
