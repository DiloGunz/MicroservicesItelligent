namespace AuthService.Application.Models.AppUserCommands;

public record RegisterAppUserCommand
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string RepetirPassword { get; set; }
    public string Role { get; set; }
}
