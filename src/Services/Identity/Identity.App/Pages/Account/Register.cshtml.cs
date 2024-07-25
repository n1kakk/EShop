using Identity.App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Identity.App.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IEmailService _emailService;

    public RegisterModel(UserManager<IdentityUser> userManager, IEmailService emailService)
    {
        this.userManager = userManager;
        _emailService = emailService;
    }

    [BindProperty]
    public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid) return Page();

        var user = new IdentityUser
        {
            Email = RegisterViewModel.Email,
            UserName = RegisterViewModel.Email
        };  

        var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);

        if (result.Succeeded)
        {
            var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail",
                values: new { userId = user.Id, token = confirmationToken });

            string subject = "Please confirm your email";
            string body = $"Please click on this link to confirm your email: {confirmationLink}";
                       
            await _emailService.SendEmailAsync(user.Email, subject, body);

            return Redirect("/Account/Login");
        }
        else
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }

            return Page();
        }
    }
}

public class RegisterViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
