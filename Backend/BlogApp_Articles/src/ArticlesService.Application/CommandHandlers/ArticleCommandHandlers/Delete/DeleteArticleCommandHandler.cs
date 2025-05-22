using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Delete;

public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, ErrorOr<Unit>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteArticleCommandHandler> _logger;

    public DeleteArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork, ILogger<DeleteArticleCommandHandler> logger)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _articleRepository.SingleOrDefaultAsNoTrackingAsync(x => x.Id == request.Id);

            if (data == null)
            {
                return Error.NotFound("Article", "El articulo seleccionado no existe.");
            }

            _articleRepository.Remove(data);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}