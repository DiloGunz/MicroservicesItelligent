using BlogApp.Shared.ContextAccessor;
using Microsoft.Extensions.Configuration;

namespace BlogApp.Proxy.AuthService.Config;

public class AuthIdentityHttpClient
{
    public HttpClient HttpClient { get; }

	public AuthIdentityHttpClient(HttpClient client, IConfiguration config, IHttpContextAccesorProvider httpContextAccesorProvider)
	{
        HttpClient = client;

        HttpClient.BaseAddress = new Uri(config["UrlServices:UrlAuthService"] ?? throw new ArgumentNullException("UrlServices:UrlAuthService"));
        HttpClient.Timeout = new TimeSpan(0, 0, 30);

        HttpClient.DefaultRequestHeaders.Clear();

        var token = httpContextAccesorProvider.GetAccessToken();

        if (!string.IsNullOrEmpty(token))
        {
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
        }
    }
}