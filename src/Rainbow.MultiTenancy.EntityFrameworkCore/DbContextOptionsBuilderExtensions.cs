using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.EntityFrameworkCore;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseMultiTenancy(this DbContextOptionsBuilder options)
        {
            return options.AddInterceptors(new MultiTenancyConnectionInterceptor(options.Options.FindExtension<CoreOptionsExtension>().ApplicationServiceProvider));
        }
    }
}
