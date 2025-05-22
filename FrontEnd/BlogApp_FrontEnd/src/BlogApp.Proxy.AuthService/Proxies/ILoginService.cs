using AuthService.Application.Models.Login;

namespace BlogApp.Proxy.AuthService.Proxies;

public interface ILoginService
{
    Task<LoginResponse> LoginAsync(LoginCommand command);
}