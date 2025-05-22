using ArticlesService.Application.Models.Commands.ArticleCommands;
using ArticlesService.Domain;
using ArticlesService.Repository.Abstraction;
using ArticlesService.UnitOfWork.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.Create;

public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ErrorOr<long>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateArticleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateArticleCommandHandler(IArticleRepository articleRepository, IUnitOfWork unitOfWork, ILogger<CreateArticleCommandHandler> logger, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<long>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _mapper.Map<Article>(request);

            await _articleRepository.AddAsync(entity);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}