using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System.ComponentModel.DataAnnotations;

namespace Identity.App.Pages.Account;

[Authorize]
public class AuthrnticatorWithMFASetupModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    [BindProperty]
    public SetupMFAViewModel ViewModel{ get; set; }

    [BindProperty]
    public bool Succeeded { get; set; } 

    public AuthrnticatorWithMFASetupModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        this.ViewModel = new SetupMFAViewModel();
        this.Succeeded = false;
    }
    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(base.User);

        if (user != null) 
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            this.ViewModel.Key = key??string.Empty;
            this.ViewModel.QRCodeBytes = GenerateQRCodeBytes("EShop", this.ViewModel.Key, user.Email??string.Empty);
        }   
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.GetUserAsync(base.User);
        if (user != null && await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider,
            this.ViewModel.SecurityCode))
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            this.Succeeded = true;
        }
        else
        {
            ModelState.AddModelError("AuthenticatorSetup", "Something went wrong with the authenticator setup");
        }
        return Page();
    }

    private Byte[] GenerateQRCodeBytes(string provider, string key, string userEmail)
    {
        var qrCodeGenerator = new QRCodeGenerator();
        var qrCodeData = qrCodeGenerator.CreateQrCode(
    $"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}",
    QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}

public class SetupMFAViewModel
{
    public string? Key { get; set; }

    [Required]
    [Display(Name ="Code")]
    public string SecurityCode { get; set; } = string.Empty;

    public Byte[]? QRCodeBytes { get; set; }
}
