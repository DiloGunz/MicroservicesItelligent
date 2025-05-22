using ArticlesService.Application.Models.Commands.CommentCommands;
using FluentValidation;

namespace ArticlesService.Application.CommandHandlers.CommentCommandhandlers.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommentCommand>
{
	public CreateCommandValidator()
	{
		RuleFor(x => x.Content).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar comentario");
		RuleFor(x => x.ArticleId).GreaterThan(0).WithMessage("Seleccionar un articulo válido");
	}
}