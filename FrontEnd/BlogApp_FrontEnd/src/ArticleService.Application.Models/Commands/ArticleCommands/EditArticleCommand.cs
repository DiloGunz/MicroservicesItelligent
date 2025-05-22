namespace ArticleService.Application.Models.Commands.ArticleCommands;

public record EditArticleCommand
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }
}