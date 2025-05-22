namespace AuthService.Application.Models.Login;

public record LoginCommand
{
    public string Username { get; set; }
    public string Password { get; set; }
}