using Rainbow.MultiTenancy.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddMultiTenancy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MultiTenancyMiddleware>();
        }
    }
}
