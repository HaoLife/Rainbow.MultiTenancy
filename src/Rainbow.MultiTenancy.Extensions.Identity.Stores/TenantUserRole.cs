using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantUserRole : TenantUserRole<string>
    {

    }
    public class TenantUserRole<TKey> : IdentityUserRole<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
