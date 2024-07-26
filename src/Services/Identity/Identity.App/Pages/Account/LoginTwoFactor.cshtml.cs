using Identity.App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Identity.App.Pages.Account;

public class LoginTwoFactorModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty]
    public EmailMFA EmailMFA { get; set; }

    public LoginTwoFactorModel(UserManager<IdentityUser> userManager, 
        IEmailService emailService, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _emailService = emailService;
        _signInManager = signInManager;
        this.EmailMFA = new EmailMFA();
    }
    public async Task OnGetAsync(string email, bool rememberMe)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            ModelState.AddModelError("Login", "Failed to login");
            //return Page();
        }

        var securityCode = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        this.EmailMFA.SecurityCode = string.Empty;
        this.EmailMFA.RememberMe = rememberMe;

        await _emailService.SendEmailAsync(email, 
            "EShop's OTP", $"Please use this code as the OTP: {securityCode}");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid) return Page();

        var result = await _signInManager.TwoFactorSignInAsync("Email", EmailMFA.SecurityCode, EmailMFA.RememberMe, false);
        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login2FA", "You are logged out");
            }
            else
            {
                ModelState.AddModelError("Login2FA", "Failed to login");
            }
            return Page();
        }
    }
}

public class EmailMFA
{
    [Required]
    [Display(Name ="SecurityCode")]
    public string SecurityCode { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}