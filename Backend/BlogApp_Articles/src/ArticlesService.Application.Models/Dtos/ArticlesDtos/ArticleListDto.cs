namespace ArticlesService.Application.Models.Dtos.ArticlesDtos;

public record ArticleListDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public long CreatedBy { get; set; }
}