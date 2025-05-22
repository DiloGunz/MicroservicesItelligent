using ArticlesService.Application.Models.Dtos.ArticlesDtos;
using ArticlesService.Application.Models.Queries.ArticleQueries;
using ArticlesService.Repository.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArticlesService.Application.CommandHandlers.ArticleCommandHandlers.GetById;

public class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ErrorOr<ArticleDetailsDto>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ILogger<GetArticleByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository, ILogger<GetArticleByIdQueryHandler> logger, IMapper mapper)
    {
        _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<ArticleDetailsDto>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _articleRepository.SingleOrDefaultAsNoTrackingAsync(x => x.Id == request.Id);

            if (data == null)
            {
                return Error.NotFound("Article", "El articulo seleccionado no existe.");
            }

            return _mapper.Map<ArticleDetailsDto>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Failure("Exception", ex.Message);
        }
    }
}