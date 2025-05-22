using AuthService.Application.Models.Login;
using BlogApp.Proxy.AuthService.Config;
using BlogApp.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace BlogApp.Proxy.AuthService.Proxies
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginService> _logger;

        public LoginService(AuthHttpClient authHttpClient, ILogger<LoginService> logger)
        {
            _httpClient = authHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authHttpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<LoginResponse> LoginAsync(LoginCommand command)
        {
            try
            {
                var contentPost = new StringContent(
                    JsonSerializer.Serialize(command),
                    Encoding.UTF8,
                    "application/json"
                );

                var url = "/api/auth/login";

                var response = await _httpClient.PostAsync(url, contentPost);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<LoginResponse>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return result;
                }

                var errors = await ApiErrorParser.GetErrorMessages(response);
                var errorResult = new LoginResponse
                {
                    Success = false,
                    Message = string.Join("\n", errors)
                };
                return errorResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new LoginResponse { Success = false, Message = ex.Message };
            }
        }
    }
}
