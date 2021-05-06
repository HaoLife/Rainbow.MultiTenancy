using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public interface ITenantHandlerStore<TUser>
        where TUser : class
    {
        Task SetTenant(TUser user, Guid? tenantId, CancellationToken cancellationToken);
    }
}
