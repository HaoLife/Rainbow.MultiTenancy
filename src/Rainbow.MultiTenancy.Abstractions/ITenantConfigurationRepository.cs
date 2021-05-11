using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantConfigurationRepository
    {
        Task<List<TenantConfigurationString>> FindByTenantIdAsync(
            Guid tenantId,
            CancellationToken cancellationToken = default
        );

    }
}
