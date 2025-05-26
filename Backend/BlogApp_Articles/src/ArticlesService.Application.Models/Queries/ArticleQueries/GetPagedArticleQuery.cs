using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.SharedKernel.Collection;
using ErrorOr;
using MediatR;

namespace ArticlesService.Application.Models.Queries.ArticleQueries;

public record GetPagedArticleQuery : IRequest<ErrorOr<DataCollection<ArticleListDto>>>
{
    public string Criteria { get; set; }
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 50;
}
