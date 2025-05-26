using AuthService.Repository.Abstraction;

namespace AuthService.UnitOfWork.Abstraction;

public interface IUnitOfWorkRepository
{
    IAppUserRepository AppUserRepository { get; }
}