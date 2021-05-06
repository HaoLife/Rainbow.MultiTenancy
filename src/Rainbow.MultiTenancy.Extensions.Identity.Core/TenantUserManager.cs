using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantUserManager<TUser> : UserManager<TUser>
          where TUser : class
    {
        public TenantUserManager(IUserStore<TUser> store
            , IOptions<IdentityOptions> optionsAccessor
            , IPasswordHasher<TUser> passwordHasher
            , IEnumerable<IUserValidator<TUser>> userValidators
            , IEnumerable<IPasswordValidator<TUser>> passwordValidators
            , ILookupNormalizer keyNormalizer
            , IdentityErrorDescriber errors
            , IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public virtual bool SupportsTenant
        {
            get
            {
                ThrowIfDisposed();
                return Store is ITenantHandlerStore<TUser>;
            }
        }

    }
}
