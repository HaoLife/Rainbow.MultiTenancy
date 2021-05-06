using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantRole : TenantRole<string>
    {
        public TenantRole()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
    public class TenantRole<TKey> : IdentityRole<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
