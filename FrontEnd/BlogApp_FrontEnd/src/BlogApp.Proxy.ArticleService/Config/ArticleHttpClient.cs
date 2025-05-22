using BlogApp.Shared.ContextAccessor;
using Microsoft.Extensions.Configuration;

namespace BlogApp.Proxy.ArticleService.Config;

public class ArticleHttpClient
{
    public HttpClient HttpClient { get; }

	public ArticleHttpClient(HttpClient client, IConfiguration config, IHttpContextAccesorProvider httpContextAccesorProvider)
	{
        HttpClient = client;

        HttpClient.BaseAddress = new Uri(config["UrlServices:UrlArticleService"] ?? throw new ArgumentNullException("UrlServices:UrlArticleService"));
        HttpClient.Timeout = new TimeSpan(0, 0, 30);

        HttpClient.DefaultRequestHeaders.Clear();

        var token = httpContextAccesorProvider.GetAccessToken();

        if (!string.IsNullOrEmpty(token))
        {
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
        }
    }
}