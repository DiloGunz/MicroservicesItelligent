using AuthService.Application.Models.AppUserCommands;
using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.AppUserQueries;
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
    private readonly HttpClient _httpIdentityClient;

    public UserProxy(AuthHttpClient authHttpClient, AuthIdentityHttpClient authIdentityHttpClient, ILogger<UserProxy> logger)
    {
        _httpClient = authHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authHttpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpIdentityClient = authIdentityHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authIdentityHttpClient));
    }

    public async Task<List<AppUserDto>> GetByIdsAsync(GetAppUserByIdsQuery query)
    {
        try
        {
            try
            {
                var response = await _httpIdentityClient.PostAsJsonAsync($"/api/auth/GetByIds", query);
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<AppUserDto>>(
                            await response.Content.ReadAsStringAsync(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null)
                    {
                        return result;
                    }
                }

                return new List<AppUserDto>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<SimpleResponse<AppUserDto>> GetMeAsync()
    {
        try
        {
            try
            {
                var response = await _httpIdentityClient.GetAsync($"/api/auth/me");
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<AppUserDto>(
                            await response.Content.ReadAsStringAsync(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (result != null)
                    {
                        return new SimpleResponse<AppUserDto>().Success(result);
                    }
                }

                return new SimpleResponse<AppUserDto>().Failure("Error al obtener datos del usuario");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
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