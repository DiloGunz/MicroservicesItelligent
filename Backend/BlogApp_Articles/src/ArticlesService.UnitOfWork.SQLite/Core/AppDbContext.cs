using ArticlesService.Domain.Base;
using ArticlesService.SharedKernel.ContextAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Security.Authentication;

namespace ArticlesService.UnitOfWork.SQLite.Core;

public class AppDbContext : DbContext
{
    protected readonly IHttpContextAccesorProvider _authService;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccesorProvider httpContextAccesorProvider) :
            base(options)
    {
        _authService = httpContextAccesorProvider ?? throw new ArgumentNullException(nameof(httpContextAccesorProvider));
	}


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly(),
                t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            );

        var relationship = builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());

        foreach (var r in relationship)
        {
            r.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await MakeAuditAsync(ChangeTracker);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private Task MakeAuditAsync(ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();

        IEnumerable<EntityEntry> entities =
        changeTracker
            .Entries()
            .Where(t => t.Entity is AuditEntity &&
            (
                t.State == EntityState.Deleted
                || t.State == EntityState.Added
                || t.State == EntityState.Modified
            ));

        if (!entities.Any())
        {
            return Task.CompletedTask;
        }

        DateTime timestamp = DateTime.Now;

        var userId = _authService.GetUserId();

        if (userId <= 0)
        {
            throw new AuthenticationException("Error de autenticación");
        }

        foreach (EntityEntry entry in entities)
        {
            if (entry.Entity is not AuditEntity entity)
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = timestamp;
                    entity.CreatedBy = userId;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = timestamp;
                    entity.UpdatedBy = userId;
                    break;
            }
        }

        return Task.CompletedTask;
    }
}