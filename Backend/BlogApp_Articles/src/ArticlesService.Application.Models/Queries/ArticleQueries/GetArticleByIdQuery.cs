using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Queries.ArticleQueries;

public record GetArticleByIdQuery : IRequest<ErrorOr<ArticleDetailsDto>>
{
    public long Id { get; set; }
}