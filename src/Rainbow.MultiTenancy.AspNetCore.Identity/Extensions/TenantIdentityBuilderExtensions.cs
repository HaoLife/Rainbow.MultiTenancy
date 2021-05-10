using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityBuilderExtensions
    {
        public static IdentityBuilder AddTenantSignInManager(this IdentityBuilder builder)
        {
            //builder.AddSignInManager();
            var managerType = typeof(SignInManager<>).MakeGenericType(builder.UserType);
            var tenantManagerType = typeof(TenantSignInManager<>).MakeGenericType(builder.UserType);

            //var t = builder.Services.LastOrDefault(a => a.ServiceType == managerType);
            //builder.Services.Remove(t);
            //builder.Services.AddScoped(managerType, tenantManagerType);
            builder.Services.AddScoped(tenantManagerType);
            var t2 = builder.Services.LastOrDefault(a => a.ServiceType == managerType);
            return builder;
        }
    }
}
