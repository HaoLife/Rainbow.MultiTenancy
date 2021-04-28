using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class MultiTenancyCoreOptions
    {

        public string TenantKey { get; set; }
        public List<ITenantResolveContributor> TenantResolvers { get; }
        public TenantConfiguration[] Tenants { get; set; }

        public MultiTenancyCoreOptions()
        {
            TenantKey = TenantResolverConsts.DefaultTenantKey;
            Tenants = new TenantConfiguration[0];
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor()
            };
        }
    }
}
