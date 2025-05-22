using BlogApp.Proxy.ArticleService.Proxies;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Proxy.ArticleService.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddArticleService(this IServiceCollection services)
    {
        services.AddHttpClient<ArticleHttpClient>();
        services.AddTransient<IArticleProxy, ArticleProxy>();
        return services;
    }
}