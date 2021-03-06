using Microsoft.AspNetCore.Identity;
using Rainbow.MultiTenancy.Abstractions;
using System;

namespace Rainbow.MultiTenancy.Extensions.Identity.Stores
{
    public class TenantUser : TenantUser<string>
    {
        public TenantUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }

    public class TenantUser<TKey> : IdentityUser<TKey>, IMultiTenant
        where TKey : IEquatable<TKey>
    {

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        public TenantUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public TenantUser(string userName) : this()
        {
            UserName = userName;
        }
        [PersonalData]
        public Guid? TenantId { get; set; }
    }
}
