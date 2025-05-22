using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Commands.ArticleCommands;

public record CreateArticleCommand : IRequest<ErrorOr<long>>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }

}