namespace ArticleService.Application.Models.Commands.ArticleCommands;

public record CreateArticleCommand
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Summary { get; set; }
}