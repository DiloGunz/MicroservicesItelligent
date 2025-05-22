namespace ArticleService.Application.Models.Dtos.CommentDtos;

public record CommentDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public string Article { get; set; }
    public long ArticleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }

}