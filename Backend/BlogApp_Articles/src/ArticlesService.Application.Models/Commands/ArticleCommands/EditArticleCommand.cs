using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Commands.ArticleCommands;

public record EditArticleCommand : IRequest<ErrorOr<long>>
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }
}