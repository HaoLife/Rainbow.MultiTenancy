using Microsoft.Extensions.DependencyInjection;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class CurrentUserTenantResolveContributor : ITenantResolveContributor
    {
        public const string ContributorName = "CurrentUser";
        public string Name => ContributorName;

        public Task ResolveAsync(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (currentUser.IsAuthenticated)
            {
                context.Handled = true;
                context.TenantIdOrName = currentUser.TenantId?.ToString();
            }

            return Task.CompletedTask;
        }
    }
}
