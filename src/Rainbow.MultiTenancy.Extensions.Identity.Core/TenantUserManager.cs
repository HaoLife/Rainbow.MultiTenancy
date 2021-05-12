using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Rainbow.MultiTenancy.Core;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantUserManager<TUser> : UserManager<TUser>
          where TUser : class
    {
        private IServiceProvider _services;
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
            this._services = services;
        }

        public virtual bool SupportsTenant
        {
            get
            {
                ThrowIfDisposed();
                return Store is ITenantHandlerStore<TUser> && Store is ITenantUserQueryStore<TUser>;
            }
        }

        public virtual async Task<Guid?> GetTanantIdAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!this.SupportsTenant) return null;

            var handler = this.Store as ITenantHandlerStore<TUser>;

            return await handler.GetTanantAsync(user, CancellationToken);
        }

        public virtual async Task<TUser> FindByNameAsync(string userName, Guid? tenantId)
        {

            ThrowIfDisposed();
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }
            userName = NormalizeName(userName);

            if (!this.SupportsTenant)
            {
                throw new NotSupportedException();
            }

            var query = this.Store as ITenantUserQueryStore<TUser>;

            var user = await query.FindByNameAsync(userName, tenantId, CancellationToken);

            // Need to potentially check all keys
            if (user == null && Options.Stores.ProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, userName);
                        user = await query.FindByNameAsync(oldKey, tenantId, CancellationToken);
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            return user;
        }

        public virtual async Task<TUser> FindByEmailAsync(string email, Guid? tenantId)
        {
            ThrowIfDisposed();
            if (!this.SupportsTenant)
            {
                throw new NotSupportedException();
            }

            if (!this.SupportsTenant)
            {
                throw new NotSupportedException();
            }

            var query = this.Store as ITenantUserQueryStore<TUser>;

            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            email = NormalizeEmail(email);
            var user = await query.FindByEmailAsync(email, tenantId, CancellationToken);

            // Need to potentially check all keys
            if (user == null && Options.Stores.ProtectPersonalData)
            {
                var keyRing = _services.GetService<ILookupProtectorKeyRing>();
                var protector = _services.GetService<ILookupProtector>();
                if (keyRing != null && protector != null)
                {
                    foreach (var key in keyRing.GetAllKeyIds())
                    {
                        var oldKey = protector.Protect(key, email);
                        user = await query.FindByEmailAsync(oldKey, tenantId, CancellationToken);
                        if (user != null)
                        {
                            return user;
                        }
                    }
                }
            }
            return user;
        }

        public virtual Task<TUser> FindByIdAsync(string userId, Guid? tenantId)
        {
            ThrowIfDisposed();
            var query = this.Store as ITenantUserQueryStore<TUser>;

            return query.FindByIdAsync(userId, tenantId, CancellationToken);
        }


        public virtual string GetTenantId(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            return principal.FindFirstValue(IdentityClaimTypes.TenantId);
        }

        public override Task<TUser> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            string userId = GetUserId(principal);
            var tenantId = GetTenantId(principal);
            if (userId != null && tenantId != null)
            {
                return FindByIdAsync(userId, Guid.Parse(tenantId));
            }
            if (userId != null)
            {
                return FindByIdAsync(userId);
            }

            return Task.FromResult<TUser>(null);
        }

    }
}
