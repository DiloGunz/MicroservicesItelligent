using AuthService.Application.Models.AppUserCommands;
using AuthService.Domain;
using AuthService.Repository.Abstraction;
using AuthService.UnitOfWork.Abstraction;
using ErrorOr;
using MediatR;

namespace AuthService.Application.AppUserCommandHandlers.Register;

public class RegisterAppUserCommandHandler : IRequestHandler<RegisterAppUserCommand, ErrorOr<long>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppUserRepository _appUserRepository;

    public RegisterAppUserCommandHandler(IUnitOfWork unitOfWork, IAppUserRepository appUserRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _appUserRepository = appUserRepository ?? throw new ArgumentNullException(nameof(appUserRepository));
    }

    public async Task<ErrorOr<long>> Handle(RegisterAppUserCommand request, CancellationToken cancellationToken)
    {
        var userDb = await _appUserRepository.AnyAsync(x => x.Username.ToUpper().Trim() == request.Username.Trim().ToUpper());
        if (userDb)
        {
            return Error.Conflict("Usuario.Username", "El nombre de usuario ingresado ya existe.");
        }

        var entity = new AppUser()
        {
            Username = request.Username.ToUpper().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role.ToLower().Trim()
        };

        await _appUserRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return entity.Id;
    }
}