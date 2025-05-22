using ArticlesService.Domain.Base;

namespace ArticlesService.Domain;

public class Comment : AuditEntity
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long ArticleId { get; set; }
    public Article Article { get; set; }
}