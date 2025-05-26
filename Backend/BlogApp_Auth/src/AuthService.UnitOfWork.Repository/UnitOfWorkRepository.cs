using AuthService.Repository.Abstraction;
using AuthService.UnitOfWork.Abstraction;

namespace AuthService.UnitOfWork.Repository;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
	public UnitOfWorkRepository(IAppUserRepository appUserRepository)
	{
		AppUserRepository = appUserRepository;
	}
    public IAppUserRepository AppUserRepository { get; }
}