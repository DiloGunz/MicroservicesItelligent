using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.SharedKernel.Collection;
using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Queries.ArticleQueries;

public record GetPagedLastArticleQuery : IRequest<ErrorOr<DataCollection<ArticleDetailsDto>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 50;
}