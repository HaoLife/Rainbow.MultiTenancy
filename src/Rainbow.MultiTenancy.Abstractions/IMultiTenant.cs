using System;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}
