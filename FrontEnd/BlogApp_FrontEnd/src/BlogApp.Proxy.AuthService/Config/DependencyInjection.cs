using BlogApp.Proxy.AuthService.Proxies;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Proxy.AuthService.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthService(this IServiceCollection services)
    {
        services.AddHttpClient<AuthHttpClient>();
        services.AddHttpClient<AuthIdentityHttpClient>();
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IUserProxy, UserProxy>();
        return services;
    }
}