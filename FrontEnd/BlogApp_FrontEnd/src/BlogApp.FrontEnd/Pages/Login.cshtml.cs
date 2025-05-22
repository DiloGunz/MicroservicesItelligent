using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.Login;
using BlogApp.Proxy.AuthService.Proxies;
using BlogApp.Shared.ContextAccessor;
using BlogApp.Shared.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace BlogApp.FrontEnd.Pages;

[IgnoreAntiforgeryToken]
public class LoginModel : PageModel
{
    private readonly ILoginService _loginService;
    private readonly IUserProxy _userProxy;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(ILoginService loginService, IUserProxy userProxy, ILogger<LoginModel> logger)
    {
        _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        _userProxy = userProxy ?? throw new ArgumentNullException(nameof(userProxy));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public LoginCommand LoginCmd { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostIdentityAsync([FromBody] LoginCommand input)
    {
        var result = await _loginService.LoginAsync(input);
        if (result.Success)
        {
            await ConnectAsync(result.Token);
        }
        return new JsonResult(result);
    }

    public async Task<IActionResult> OnPostRegisterAsync([FromBody] RegisterAppUserCommand input)
    {
        var response = await _userProxy.RegisterAsync(input);
        return new JsonResult(response);
    }

    private async Task ConnectAsync(string accessToken)
    {
        try
        {
            var token = accessToken.Split('.');

            var payload = token[1].DecodeBase64Url();
            var user = JsonSerializer.Deserialize<AccessTokenUserInformation>(payload);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.sub),
                new Claim(ClaimTypes.Name, user.email),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("access_token", accessToken),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.Now.AddDays(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}