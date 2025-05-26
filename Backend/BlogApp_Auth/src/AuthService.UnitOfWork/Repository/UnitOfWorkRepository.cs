using AuthService.Repository.Abstraction;
using AuthService.UnitOfWork.Abstractions;

namespace AuthService.UnitOfWork.Repository;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
	public UnitOfWorkRepository(IUsuarioRepository usuarioRepository)
	{
		UsuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
	}
    public IUsuarioRepository UsuarioRepository { get; }
}