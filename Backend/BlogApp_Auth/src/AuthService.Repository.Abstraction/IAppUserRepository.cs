using AuthService.Domain;
using AuthService.Repository.Abstraction.Generic;

namespace AuthService.Repository.Abstraction;

public interface IAppUserRepository : 
    IReadRepository<AppUser>,
    ICreateRepository<AppUser>
{

}