using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public static class TenantResolveContextExtensions
    {
        public static MultiTenancyCoreOptions GetMultiTenancyCoreOptions(this ITenantResolveContext context)
        {
            return context.ServiceProvider.GetRequiredService<IOptionsSnapshot<MultiTenancyCoreOptions>>().Value;
        }
    }
}
