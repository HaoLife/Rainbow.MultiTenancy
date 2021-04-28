using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class NullTenantResolveResultAccessor : ITenantResolveResultAccessor
    {
        public TenantResolveResult Result
        {
            get => null;
            set { }
        }

    }
}
