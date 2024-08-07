using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Identity.App.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<IdentityUser> userManager;
    [BindProperty]
    public string Message { get; set; } = string.Empty;

    public ConfirmEmailModel(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
    public async Task<IActionResult> OnGetAsync(string userId, string token)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if(user is not null)
        {
            var result = await this.userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                this.Message = "Email address is confirmed, you can now log in.";
                return Page();
            }
        }

        this.Message = "Failed to validate email";
        return Page();
    }
}
