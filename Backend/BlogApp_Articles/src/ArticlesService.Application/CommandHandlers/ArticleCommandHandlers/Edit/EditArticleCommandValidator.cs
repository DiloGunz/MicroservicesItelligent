using ArticlesService.Application.Models.Commands.ArticleCommands;
using FluentValidation;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Edit;

public class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
{
	public EditArticleCommandValidator()
	{
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id inválido.");
        RuleFor(x => x.Title).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar titulo.");
        RuleFor(x => x.Content).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Ingresar contenido.");
    }
}