using ArticleService.Application.Models.Commands.CommentCommands;
using ArticleService.Application.Models.Dtos.CommentDtos;
using BlogApp.Proxy.ArticleService.Config;
using BlogApp.Shared.Responses;
using BlogApp.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlogApp.Proxy.ArticleService.Proxies;

public class CommentProxy : ICommentProxy
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ArticleProxy> _logger;

    public CommentProxy(ArticleHttpClient authHttpClient, ILogger<ArticleProxy> logger)
    {
        _httpClient = authHttpClient.HttpClient ?? throw new ArgumentNullException(nameof(authHttpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SimpleResponse<long>> CreateAsync(CreateCommentCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/articles/{command.ArticleId}/comments", command);
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

    public async Task<List<CommentDto>> GetByArticleAsync(long articleId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/comment/GetByArticleId/{articleId}");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<List<CommentDto>>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }

            return new List<CommentDto>();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}