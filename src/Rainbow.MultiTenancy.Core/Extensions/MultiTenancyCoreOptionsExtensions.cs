using Microsoft.Extensions.Configuration;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyCoreOptionsExtensions
    {
        public static MultiTenancyCoreOptions AddDefaultTenantConfiguration(this MultiTenancyCoreOptions options, IConfiguration configuration)
        {
            var tenants = configuration.Get<TenantConfiguration[]>();
            options.Tenants = tenants.Concat(options.Tenants).ToArray();
            return options;
        }

    }
}
