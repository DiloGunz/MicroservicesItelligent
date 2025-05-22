using ArticlesService.Domain.Base;

namespace ArticlesService.Domain;

public class Article : AuditEntity
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public ICollection<Comment> Comments { get; set; }
}