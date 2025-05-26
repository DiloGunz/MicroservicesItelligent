namespace AuthService.Domain;

public class AppUser
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}