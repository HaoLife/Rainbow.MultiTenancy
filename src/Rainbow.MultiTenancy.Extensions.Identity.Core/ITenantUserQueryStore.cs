using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public interface ITenantUserQueryStore<TUser>
        where TUser : class
    {
        Task<TUser> FindByIdAsync(string userId, Guid? tenantId, CancellationToken cancellationToken);

        Task<TUser> FindByNameAsync(string normalizedUserName, Guid? tenantId, CancellationToken cancellationToken);

        Task<TUser> FindByEmailAsync(string normalizedUserName, Guid? tenantId, CancellationToken cancellationToken);

        Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, Guid? tenantId, CancellationToken cancellationToken = default);
    }
}
