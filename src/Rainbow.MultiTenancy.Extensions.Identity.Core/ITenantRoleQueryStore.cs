using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public interface ITenantRoleQueryStore<TRole> : IDisposable where TRole : class
    {
        Task<TRole> FindByIdAsync(string id, Guid? tenantId, CancellationToken cancellationToken);

        Task<TRole> FindByNameAsync(string normalizedRoleName, Guid? tenantId, CancellationToken cancellationToken);

    }
}
