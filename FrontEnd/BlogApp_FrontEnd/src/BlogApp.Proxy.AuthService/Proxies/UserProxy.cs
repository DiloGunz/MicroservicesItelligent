using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.Login;
using BlogApp.Proxy.AuthService.Config;
using BlogApp.Shared.Responses;
using BlogApp.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlogApp.Proxy.AuthService.Proxies;

public class UserProxy : IUserProxy
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserProxy> _logger;

    public UserProxy(AuthHttpClient authHttpClient, ILogger<UserProxy> logger)
    {
        _httpClient = authHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authHttpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SimpleResponse<AppUserDto>> GetByIdAsync(long id)
    {
        try
        {
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SimpleResponse<AppUserDto>().Failure(ex.Message);
        }
    }

    public async Task<SimpleResponse<long>> RegisterAsync(RegisterAppUserCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", command);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<long>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new SimpleResponse<long>().Success(result);
            }

            return new SimpleResponse<long>().Failure(await ApiErrorParser.GetErrorMessages(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new SimpleResponse<long>().Failure(ex.Message);
        }
    }
}