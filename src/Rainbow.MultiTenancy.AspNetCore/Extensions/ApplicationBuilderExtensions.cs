using Rainbow.MultiTenancy.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenancyMiddleware>();
        }

        public static IApplicationBuilder AddMultiTenancyService(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenancyServiceMiddleware>();
        }
    }
}
