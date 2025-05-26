using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.AppUserQueries;
using AuthService.Repository.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.AppUserCommandHandlers.GetByIds;

public class GetAppUserByIdsQueryHandler : IRequestHandler<GetAppUserByIdsQuery, ErrorOr<List<AppUserDto>>>
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAppUserByIdsQueryHandler> _logger;

    public GetAppUserByIdsQueryHandler(IMapper mapper, IAppUserRepository appUserRepository, ILogger<GetAppUserByIdsQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _appUserRepository = appUserRepository ?? throw new ArgumentNullException(nameof(appUserRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<ErrorOr<List<AppUserDto>>> Handle(GetAppUserByIdsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _appUserRepository.GetAllAsNoTrackingAsync(x => request.Ids.Contains(x.Id));
            return _mapper.Map<List<AppUserDto>>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}