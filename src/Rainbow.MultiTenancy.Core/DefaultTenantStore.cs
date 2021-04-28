using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class DefaultTenantStore : ITenantStore
    {
        private readonly MultiTenancyCoreOptions _options;

        public DefaultTenantStore(IOptionsSnapshot<MultiTenancyCoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<TenantConfiguration> FindAsync(string name)
        {
            return Task.FromResult(_options.Tenants?.FirstOrDefault(t => t.Name == name));
        }

        public Task<TenantConfiguration> FindAsync(Guid id)
        {
            return Task.FromResult(_options.Tenants?.FirstOrDefault(t => t.Id == id));
        }
    }
}
