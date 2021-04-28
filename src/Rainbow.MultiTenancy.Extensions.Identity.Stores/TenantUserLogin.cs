using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantUserLogin : TenantUserLogin<string>
    {

    }
    public class TenantUserLogin<TKey> : IdentityUserLogin<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
