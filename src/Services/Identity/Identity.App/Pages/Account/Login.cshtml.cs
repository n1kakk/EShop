using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


namespace Identity.App.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> signInManager;
    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }


    [BindProperty]
    public CredentialViewModel Credential { get; set; } = new CredentialViewModel();
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var result = await signInManager.PasswordSignInAsync(
            this.Credential.Email,
            this.Credential.Password,
            this.Credential.RememberMe,
            false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("/Account/LoginTwoFactorWithAuthenticatorModel", 
                new {
                    Email = this.Credential.Email,
                    RememberMe = this.Credential.RememberMe
                });
            }

            if (result.IsLockedOut){
                ModelState.AddModelError("Login", "You are logged out");
            }
            else
            {
                ModelState.AddModelError("Login", "Failed to login");
            }
            return Page();
        }

    }

}

public class CredentialViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Renenber me")]
    public bool RememberMe { get; set; }
}