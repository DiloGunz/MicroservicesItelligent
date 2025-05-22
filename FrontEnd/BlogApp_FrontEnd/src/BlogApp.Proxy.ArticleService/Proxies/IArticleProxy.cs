using ArticleService.Application.Models.Commands.ArticleCommands;
using ArticleService.Application.Models.Dtos.ArticlesDtos;
using ArticleService.Application.Models.Queries.ArticleQueries;
using BlogApp.Shared.Collection;
using BlogApp.Shared.Responses;

namespace BlogApp.Proxy.ArticleService.Proxies;

public interface IArticleProxy
{
    Task<DataCollection<ArticleListDto>> GetPagedAsync(GetPagedArticleQuery query);
    Task<SimpleResponse<long>> CreateAsync(CreateArticleCommand command);
}