using BlogApp.FrontEnd.Utils;
using BlogApp.FrontEnd.Utils.Middlewares;
using BlogApp.Proxy.ArticleService.Config;
using BlogApp.Proxy.AuthService.Config;
using BlogApp.Shared.ContextAccessor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.FrontEnd.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IHttpContextAccesorProvider, HttpContextAccesorProvider>();

        services.AddTransient<GloblalExceptionHandlingMiddleware>();

        // Add services to the container.
        services.AddRazorPages(options =>
        {
            options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        }).AddRazorRuntimeCompilation();

        services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/login?handler=Logout";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
                options.SlidingExpiration = true;
            });

        services.AddAuthorizationOptions();

        services.AddAuthService();
        services.AddArticleService();

        return services;
    }

    private static IServiceCollection AddAuthorizationOptions(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser();

            options.FallbackPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        return services;
    }
}