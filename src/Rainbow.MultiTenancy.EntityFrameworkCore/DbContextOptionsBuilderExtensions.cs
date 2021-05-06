using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.EntityFrameworkCore;
using System.Linq;
using Rainbow.MultiTenancy.Abstractions;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseMultiTenancy(this DbContextOptionsBuilder options, string connName = ConnectionStrings.DefaultConnectionStringName)
        {
            return options.AddInterceptors(new MultiTenancyConnectionInterceptor(options.Options.FindExtension<CoreOptionsExtension>().ApplicationServiceProvider, connName));
        }
    }
}
