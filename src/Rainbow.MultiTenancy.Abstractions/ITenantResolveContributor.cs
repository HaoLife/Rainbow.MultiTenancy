using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantResolveContributor
    {
        string Name { get; }

        Task ResolveAsync(ITenantResolveContext context);
    }
}
