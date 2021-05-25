using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantUserToken : TenantUserToken<string>
    {

    }
    public class TenantUserToken<TKey> : IdentityUserToken<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {
        public Guid? TenantId { get; set; }
    }
}
