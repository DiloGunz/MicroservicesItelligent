using ArticleService.Application.Models.Commands.CommentCommands;
using ArticleService.Application.Models.Dtos.CommentDtos;
using BlogApp.Shared.Responses;

namespace BlogApp.Proxy.ArticleService.Proxies;

public interface ICommentProxy
{
    Task<List<CommentDto>> GetByArticleAsync(long articleId);
    Task<SimpleResponse<long>> CreateAsync(CreateCommentCommand command);
}