using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using ChatJS.WebServer;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649
namespace ChatJS.WebServer.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIntegrityService _integrityService;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIntegrityService integrityService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _integrityService = integrityService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(
                    Input.Name, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.Name);
                    await _integrityService.EnsureUserCreatedAsync(user);
                    await _integrityService.EnsureUserConfirmedAsync(user);

                    return LocalRedirect(returnUrl);
                }
                else if (signInResult.RequiresTwoFactor)
                {
                    throw new NotImplementedException();
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
