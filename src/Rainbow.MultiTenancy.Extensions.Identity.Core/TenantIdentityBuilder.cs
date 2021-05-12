using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Extensions.Identity.Core
{
    public class TenantIdentityBuilder : IdentityBuilder
    {
        public TenantIdentityBuilder(Type user, IServiceCollection services)
            : base(user, services)
        {
        }

        public TenantIdentityBuilder(Type user, Type role, IServiceCollection services)
            : base(user, role, services)
        {
        }

        public override IdentityBuilder AddRoles<TRole>()
        {
            this.SetRole(typeof(TRole));
            AddRoleValidator<TenantRoleValidator<TRole>>();
            Services.TryAddScoped<RoleManager<TRole>, TenantRoleManager<TRole>>();
            Services.TryAddScoped<TenantRoleManager<TRole>>();
            Services.AddScoped(typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(UserType), typeof(TenantUserClaimsPrincipalFactory<,>).MakeGenericType(UserType, RoleType));
            return this;
        }

        protected virtual void SetRole(Type role)
        {
            var p = typeof(IdentityBuilder).GetProperty($"{nameof(this.RoleType)}");
            p.SetValue(this, role);
        }
    }
}
