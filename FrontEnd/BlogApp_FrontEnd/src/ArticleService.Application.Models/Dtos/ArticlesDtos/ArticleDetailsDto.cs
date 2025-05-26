namespace ArticleService.Application.Models.Dtos.ArticlesDtos;

public record ArticleDetailsDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
    public string CreatedByName { get; set; }
}