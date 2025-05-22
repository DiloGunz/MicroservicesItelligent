using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Commands.ArticleCommands;

public record DeleteArticleCommand : IRequest<ErrorOr<Unit>>
{
    public long Id { get; set; }
}