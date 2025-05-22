using ArticlesService.Application.Models.Dtos.CommentDtos;
using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Queries.CommentQueries;

public record GetCommentByArticleIdQuery : IRequest<ErrorOr<List<CommentDto>>>
{
    public long ArticuloId { get; set; }
}