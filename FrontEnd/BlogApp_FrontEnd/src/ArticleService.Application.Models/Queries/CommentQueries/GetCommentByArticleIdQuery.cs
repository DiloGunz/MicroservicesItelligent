namespace ArticleService.Application.Models.Queries.CommentQueries;

public record GetCommentByArticleIdQuery 
{
    public long ArticuloId { get; set; }
}