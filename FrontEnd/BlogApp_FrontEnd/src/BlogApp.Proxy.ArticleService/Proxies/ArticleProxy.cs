using ArticleService.Application.Models.Commands.ArticleCommands;
using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Queries.ArticleQueries;
using BlogApp.Proxy.ArticleService.Config;
using BlogApp.Shared.Collection;
using BlogApp.Shared.Responses;
using BlogApp.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlogApp.Proxy.ArticleService.Proxies;

public class ArticleProxy : IArticleProxy
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ArticleProxy> _logger;

    public ArticleProxy(ArticleHttpClient authHttpClient, ILogger<ArticleProxy> logger)
    {
        _httpClient = authHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authHttpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SimpleResponse<long>> CreateAsync(CreateArticleCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/articles", command);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<long>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new SimpleResponse<long>().Success(result);
            }

            return new SimpleResponse<long>().Failure(await response.GetErrorMessages());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<SimpleResponse<dynamic>> DeleteAsync(DeleteArticleCommand command)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/articles/{command.Id}");
            if (response.IsSuccessStatusCode)
            {
                return new SimpleResponse<dynamic>().Success(true);
            }

            return new SimpleResponse<dynamic>().Failure(await response.GetErrorMessages());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<SimpleResponse<long>> EditAsync(EditArticleCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/articles/{command.Id}", command);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<long>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return new SimpleResponse<long>().Success(result);
            }

            return new SimpleResponse<long>().Failure(await response.GetErrorMessages());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<ArticleDetailsDto> GetByIdAsync(long id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/articles/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ArticleDetailsDto>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null)
                {
                    return result;
                }
            }

            return null;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<DataCollection<ArticleListDto>> GetPagedAsync(GetPagedArticleQuery query)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/articles?criteria={query.Criteria}&page={query.Page}&take={query.Take}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DataCollection<ArticleListDto>>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null)
                {
                    return result;
                }
            }

            return new DataCollection<ArticleListDto>();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<DataCollection<ArticleDetailsDto>> GetPagedLastAsync(GetPagedLastArticleQuery query)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/articles/GetPagedLast?page={query.Page}&take={query.Take}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DataCollection<ArticleDetailsDto>>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result != null)
                {
                    return result;
                }
            }

            return new DataCollection<ArticleDetailsDto>();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}