using ArticlesService.Application.Models.Commands.ArticleCommands;
using FluentValidation;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Delete;

public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
	public DeleteArticleCommandValidator()
	{
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id inválido.");
    }
}