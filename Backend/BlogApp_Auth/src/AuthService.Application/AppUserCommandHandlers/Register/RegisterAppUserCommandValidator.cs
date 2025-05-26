using AuthService.Application.Models.AppUserCommands;
using FluentValidation;

namespace AuthService.Application.AppUserCommandHandlers.Register;

public class RegisterAppUserCommandValidator : AbstractValidator<RegisterAppUserCommand>
{
    public RegisterAppUserCommandValidator()
    {
        RuleFor(x => x.Username).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar nombre de usuario");
        RuleFor(x => x.Password).Equal(x => x.RepetirPassword).WithMessage("Las contraseñas ingresadas no son iguales.");
        RuleFor(x => x.Role).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Seleccionar un rol válido");
    }
}