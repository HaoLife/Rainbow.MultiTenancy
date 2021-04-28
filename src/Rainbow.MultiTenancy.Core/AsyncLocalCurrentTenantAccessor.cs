using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class AsyncLocalCurrentTenantAccessor : ICurrentTenantAccessor
    {
        public BasicTenantInfo Current
        {
            get => _currentScope.Value;
            set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<BasicTenantInfo> _currentScope;

        public AsyncLocalCurrentTenantAccessor()
        {
            _currentScope = new AsyncLocal<BasicTenantInfo>();
        }
    }
}
