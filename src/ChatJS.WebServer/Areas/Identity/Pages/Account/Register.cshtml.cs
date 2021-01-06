using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using ChatJS.WebServer.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649
namespace ChatJS.WebServer.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IIntegrityService _integrityService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterModel(
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
                var user = new IdentityUser { UserName = Input.Name };
                var userIdentityResult = await _userManager.CreateAsync(user, Input.Password);

                if (userIdentityResult.Succeeded)
                {
                    await _integrityService.EnsureUserCreatedAsync(user);
                    await _integrityService.EnsureUserConfirmedAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                foreach (var error in userIdentityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

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

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
