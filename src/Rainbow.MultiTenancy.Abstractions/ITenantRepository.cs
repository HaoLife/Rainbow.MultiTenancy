using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantRepository
    {
        Task<Tenant> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        Task<Tenant> FindByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<Tenant>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            CancellationToken cancellationToken = default);

    }
}
