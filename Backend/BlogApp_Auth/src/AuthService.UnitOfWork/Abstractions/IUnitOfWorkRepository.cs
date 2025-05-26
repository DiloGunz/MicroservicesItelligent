using AuthService.Repository.Abstraction;

namespace AuthService.UnitOfWork.Abstractions;

public interface IUnitOfWorkRepository
{
    IUsuarioRepository UsuarioRepository { get; }
}