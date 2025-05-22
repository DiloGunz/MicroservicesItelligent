namespace ArticleService.Application.Models.Commands.CommentCommands;

public record CreateCommentCommand 
{
    public string Content { get; set; }
    public long ArticleId { get; set; }
}