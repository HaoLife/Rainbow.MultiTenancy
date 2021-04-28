using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantStore
    {
        Task<TenantConfiguration> FindAsync(string name);

        Task<TenantConfiguration> FindAsync(Guid id);
    }
}
