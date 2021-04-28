using System;

namespace Rainbow.MultiTenant.Abstractions
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}
