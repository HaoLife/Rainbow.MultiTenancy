using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.UI
{
    public class TenantIdentityDefaultUIConfigureOptions<TUser> :
        IConfigureNamedOptions<CookieAuthenticationOptions> where TUser : class
    {
        private const string IdentityUIDefaultAreaName = "Identity";

        public TenantIdentityDefaultUIConfigureOptions(
            IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public IWebHostEnvironment Environment { get; }
        public void Configure(CookieAuthenticationOptions options)
        {
            // Nothing to do here as Configure(string name, CookieAuthenticationOptions options) is hte one setting things up.
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            options = options ?? throw new ArgumentNullException(nameof(options));

            if (string.Equals(IdentityConstants.ApplicationScheme, name, StringComparison.Ordinal))
            {
                options.LoginPath = $"/{IdentityUIDefaultAreaName}/Account/TenantLogin";
            }
        }
    }
}
