using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Commands.CommentCommands;

public record CreateCommentCommand : IRequest<ErrorOr<long>>
{
    public string Content { get; set; }
    public long ArticleId { get; set; }
}