using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.Core;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;

namespace Rainbow.MultiTenancy.AspNetCore.Identity.UI.Areas.Identity.Pages.Account
{

    public class LoginModel : PageModel
    {
        private readonly SignInManager<TenantUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IOptionsSnapshot<MultiTenancyCoreOptions> options;
        private readonly ITenantStore tenantStore;

        public LoginModel(SignInManager<TenantUser> signInManager
            , ILogger<LoginModel> logger
            , ICurrentTenant currentTenant
            , IOptionsSnapshot<MultiTenancyCoreOptions> options
            , ITenantStore tenantStore)
        {
            _signInManager = signInManager;
            _logger = logger;
            CurrentTenant = currentTenant;
            this.options = options;
            this.tenantStore = tenantStore;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public ICurrentTenant CurrentTenant { get; }

        public class InputModel
        {
            [Required]
            public string TenantIdOrName { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public virtual async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;

        }


        public virtual async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (!Guid.TryParse(Input.TenantIdOrName, out var parsedTenantId))
                {
                    var tenant = await this.tenantStore.FindAsync(Input.TenantIdOrName);
                    parsedTenantId = tenant.Id;
                }

                using (this.CurrentTenant.Change(parsedTenantId))
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
