using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public class BasicTenantInfo
    {
        public Guid? TenantId { get; }
        public string Name { get; }

        public BasicTenantInfo(Guid? tenantId, string name = null)
        {
            TenantId = tenantId;
            Name = name;
        }
    }
}
