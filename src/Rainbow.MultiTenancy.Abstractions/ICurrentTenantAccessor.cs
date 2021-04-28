using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ICurrentTenantAccessor
    {
        BasicTenantInfo Current { get; set; }
    }
}
