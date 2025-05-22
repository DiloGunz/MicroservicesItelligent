namespace BlogApp.Shared.ContextAccessor;

public record CurrentUserDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
}