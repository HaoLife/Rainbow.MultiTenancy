using Rainbow.MultiTenancy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Core
{
    public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        private readonly AsyncLocal<ClaimsPrincipal> _currentPrincipal = new AsyncLocal<ClaimsPrincipal>();

        public ClaimsPrincipal Principal => _currentPrincipal.Value ?? GetClaimsPrincipal();

        public IDisposable Change(ClaimsPrincipal principal)
        {
            var parent = Principal;
            _currentPrincipal.Value = principal;
            return new DisposeAction(() =>
            {
                _currentPrincipal.Value = parent;
            });
        }


        protected virtual ClaimsPrincipal GetClaimsPrincipal()
        {
            return Thread.CurrentPrincipal as ClaimsPrincipal;
        }
    }
}
