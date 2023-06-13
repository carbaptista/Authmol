// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Authmol.Application.Services;
using Authmol.Application.Services.Email;
using Authmol.Persistence.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Authmol.UI.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IMailService _emailService;
    private readonly IUserService _userService;

    public RegisterModel(
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager,
        ILogger<RegisterModel> logger,
        IUserService userService,
        IMailService emailService)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _userService = userService;
        _emailService = emailService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();
    public string? ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    public List<string> Erros { get; set; } = new();

    public async Task OnGetAsync(string? returnUrl)
    {
        ReturnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl)
    {
        await CheckDuplicateEmail();
        if (Erros.Count > 0)
            return Page();

        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _userService.CriarEndereco(Input, user.Id);

                _logger.LogInformation("User with Id {@Id} created - {@DateTimeUtc}", user.Id, DateTime.UtcNow);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                MailData mailData = new()
                {
                    EmailBody = $"Confirme seu email clicando <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>aqui.</a>",
                    EmailSubject = $"Authmol - Confirme seu email",
                    EmailToName = Input.Email,
                    EmailToId = Input.Email
                };

                await _emailService.SendVerificationMailAsync(mailData);

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
        }

        _logger.LogError("Error creating user - {@DateTimeUtc}", DateTime.UtcNow);
        return Page();
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<IdentityUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    private async Task CheckDuplicateEmail()
    {
        var existe = await _userManager.FindByEmailAsync(Input.Email ?? "") is not null;
        if (existe)
            ModelState.AddModelError("Input.Email", "Email já cadastrado");
    }
}
