namespace ArticleService.Application.Models.Queries.ArticleQueries;

public record GetPagedArticleQuery 
{
    public string Criteria { get; set; }
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 5;
}
