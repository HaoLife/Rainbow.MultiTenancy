using Microsoft.AspNetCore.Http;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
        }
    }
}
