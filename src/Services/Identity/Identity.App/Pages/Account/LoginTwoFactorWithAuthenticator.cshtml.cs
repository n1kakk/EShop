using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Identity.App.Pages.Account;

public class LoginTwoFactorWithAuthenticatorModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty]
    public AuthenticatorMFAViewModel AuthenticatorMFA { get; set; }

    public LoginTwoFactorWithAuthenticatorModel(SignInManager<IdentityUser> signInManager)
    {
        this.AuthenticatorMFA = new AuthenticatorMFAViewModel();
        _signInManager = signInManager;
    }
    public void OnGet(bool rememberMe)
    {
        this.AuthenticatorMFA.SecurityCode = string.Empty;
        this.AuthenticatorMFA.RememberMe = rememberMe;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)  return Page();

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(this.AuthenticatorMFA.SecurityCode,
            this.AuthenticatorMFA.RememberMe, false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("AuthenticatorMFA", "You are logged out");
            }
            else
            {
                ModelState.AddModelError("AuthenticatorMFA", "Failed to login");
            }
            return Page();
        }
    }
}

public class AuthenticatorMFAViewModel
{
    [Required]
    [Display(Name = "Code")]
    public string SecurityCode { get; set; } = string.Empty;
    public bool RememberMe { get; set; }

}