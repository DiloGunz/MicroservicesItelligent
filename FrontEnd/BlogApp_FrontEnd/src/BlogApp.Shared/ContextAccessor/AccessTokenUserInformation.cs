namespace BlogApp.Shared.ContextAccessor;

public record AccessTokenUserInformation
{
    public string sub { get; set; }
    public string email { get; set; }
    public string unique_name { get; set; }
    public int exp { get; set; }
    public string user_json { get; set; }
    public string role { get; set; }
}