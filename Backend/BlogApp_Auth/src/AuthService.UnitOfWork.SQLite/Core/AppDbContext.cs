using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthService.UnitOfWork.SQLite.Core;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
    {

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
}