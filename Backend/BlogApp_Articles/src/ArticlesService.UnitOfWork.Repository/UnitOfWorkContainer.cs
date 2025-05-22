using ArticlesService.UnitOfWork.Abstraction;
using ArticlesService.UnitOfWork.SQLite.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArticlesService.UnitOfWork.Repository;

public class UnitOfWorkContainer : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWorkContainer(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
