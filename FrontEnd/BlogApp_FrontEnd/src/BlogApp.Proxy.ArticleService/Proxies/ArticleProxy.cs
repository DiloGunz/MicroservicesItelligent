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

    public async Task<DataCollection<ArticleListDto>> GetPagedAsync(GetPagedArticleQuery query)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/articles?page={query.Page}&take={query.Take}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<DataCollection<ArticleListDto>>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }

            return new DataCollection<ArticleListDto>();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}