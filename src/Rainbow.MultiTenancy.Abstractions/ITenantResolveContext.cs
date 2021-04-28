using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantResolveContext
    {
        IServiceProvider ServiceProvider { get; }
        string TenantIdOrName { get; set; }
        bool Handled { get; set; }
    }
}
