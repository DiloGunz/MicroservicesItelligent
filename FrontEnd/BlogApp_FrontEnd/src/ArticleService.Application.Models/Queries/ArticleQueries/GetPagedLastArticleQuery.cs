namespace ArticleService.Application.Models.Queries.ArticleQueries;

public record GetPagedLastArticleQuery
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 5;
}