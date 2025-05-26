using AuthService.Repository.Abstraction;
using AuthService.UnitOfWork.SQLite.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.UnitOfWork.SQLite.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddSQLite(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("AppContext");

        services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));

        return services;
    }
}