﻿using ClearSoundCompany.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace ClearSoundCompany.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReCaptcha _captcha;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ReCaptcha captcha)
        {
            _userManager = userManager;
            _emailSender = emailSender;

            _captcha = captcha;
        }

        [BindProperty] public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (!Request.Form.ContainsKey("g-recaptcha-response"))
                {
                    TempData["alertDanger"] = "Please select reCAPTCHA";
                    return Page();
                }

                var captcha = Request.Form["g-recaptcha-response"].ToString();
                if (!await _captcha.IsValid(captcha))
                {
                    TempData["alertDanger"] = "Please select reCAPTCHA";
                    return Page();
                }

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    null,
                    new { area = "Identity", code },
                    Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    callbackUrl);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        public class InputModel
        {
            [Required] [EmailAddress] public string Email { get; set; }
        }
    }
}