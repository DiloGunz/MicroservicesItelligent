using AuthService.Application.Models.AppUserDtos;
using AuthService.Application.Models.AppUserQueries;
using AuthService.Repository.Abstraction;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace AuthService.Application.AppUserCommandHandlers.GetById;

public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, ErrorOr<AppUserDto>>
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IMapper _mapper;

    public GetAppUserByIdQueryHandler(IMapper mapper, IAppUserRepository appUserRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _appUserRepository = appUserRepository ?? throw new ArgumentNullException(nameof(appUserRepository));   
    }

    public async Task<ErrorOr<AppUserDto>> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
    {
        var userDb = await _appUserRepository.SingleOrDefaultAsNoTrackingAsync(x=>x.Id == request.Id);

        if (userDb == null)
        {
            return Error.NotFound("Appuser", "El usuario no ha sido encontrado.");
        }

        var response = _mapper.Map<AppUserDto>(userDb);
        return response;
    }
}