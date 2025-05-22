namespace ArticleService.Application.Models.Commands.ArticleCommands;

public record DeleteArticleCommand
{
    public long Id { get; set; }
}