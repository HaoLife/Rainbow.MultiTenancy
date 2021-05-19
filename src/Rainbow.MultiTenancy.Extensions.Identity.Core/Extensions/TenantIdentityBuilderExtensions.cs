using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Extensions.Identity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityBuilderExtensions
    {
        public static IdentityBuilder AddTenantIdentityCore(this IdentityBuilder builder)
        {
            builder.Services.AddScoped(
                typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(builder.UserType)
                , typeof(TenantUserClaimsPrincipalFactory<>).MakeGenericType(builder.UserType));

            return builder;
        }



    }
}
