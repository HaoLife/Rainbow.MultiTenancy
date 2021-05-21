using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.AspNetCore.Identity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityBuilderUIExtensions
    {
        public static IdentityBuilder AddTenantDefaultUI(this IdentityBuilder builder)
        {
            builder.Services.ConfigureOptions(
                typeof(TenantIdentityDefaultUIConfigureOptions<>)
                    .MakeGenericType(builder.UserType));
            return builder;
        }
    }
}
