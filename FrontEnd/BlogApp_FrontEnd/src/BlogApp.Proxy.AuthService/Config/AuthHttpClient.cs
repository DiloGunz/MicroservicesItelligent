using BlogApp.Shared.ContextAccessor;
using Microsoft.Extensions.Configuration;

namespace BlogApp.Proxy.AuthService.Config;

public class AuthHttpClient
{
    public HttpClient HttpClient { get; }

	public AuthHttpClient(HttpClient client, IConfiguration config, IHttpContextAccesorProvider httpContextAccesorProvider)
	{
        HttpClient = client;

        HttpClient.BaseAddress = new Uri(config["UrlServices:UrlAuthService"] ?? throw new ArgumentNullException("UrlServices:UrlAuthService"));
        HttpClient.Timeout = new TimeSpan(0, 0, 30);

        HttpClient.DefaultRequestHeaders.Clear();
    }
}