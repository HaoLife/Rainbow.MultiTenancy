using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class TenantResolveContext : ITenantResolveContext
    {
        public IServiceProvider ServiceProvider { get; }

        public string TenantIdOrName { get; set; }

        public bool Handled { get; set; }

        public bool HasResolvedTenantOrHost()
        {
            return Handled || TenantIdOrName != null;
        }

        public TenantResolveContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
