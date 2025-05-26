using AuthService.Domain;
using AuthService.Repository.Abstraction;
using AuthService.Repository.SQLite.Generic;
using AuthService.UnitOfWork.SQLite.Core;

namespace AuthService.Repository.SQLite;

public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
{
	public AppUserRepository(AppDbContext ctx)
	{
		_context = ctx;
	}
}