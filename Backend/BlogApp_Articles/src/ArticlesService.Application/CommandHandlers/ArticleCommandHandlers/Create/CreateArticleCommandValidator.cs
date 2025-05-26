using ArticlesService.Application.Models.Commands.ArticleCommands;
using FluentValidation;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Create;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
	public CreateArticleCommandValidator()
	{
		RuleFor(x => x.Title).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar titulo.");
        RuleFor(x => x.Content).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar contenido.");
    }
}