using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Edit;

public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand, ErrorOr<long>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditArticleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public EditArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork, ILogger<EditArticleCommandHandler> logger, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<long>> Handle(EditArticleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _articleRepository.SingleOrDefaultAsync(x => x.Id == request.Id);

            if (data == null)
            {
                return Error.NotFound("Article", "El articulo seleccionado no existe.");
            }

            _mapper.Map(request, data);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return data.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}