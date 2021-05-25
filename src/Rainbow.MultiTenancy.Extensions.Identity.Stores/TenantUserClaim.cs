using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantUserClaim : TenantUserClaim<string>
    {

    }
    public class TenantUserClaim<TKey> : IdentityUserClaim<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
