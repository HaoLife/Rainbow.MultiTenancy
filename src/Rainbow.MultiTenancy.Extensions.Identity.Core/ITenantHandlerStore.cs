using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public interface ITenantHandlerStore<TModel>
        where TModel : class
    {
        Task SetTenantAsync(TModel model, Guid? tenantId, CancellationToken cancellationToken);

        Task<Guid?> GetTanantAsync(TModel model, CancellationToken cancellationToken);
    }
}
