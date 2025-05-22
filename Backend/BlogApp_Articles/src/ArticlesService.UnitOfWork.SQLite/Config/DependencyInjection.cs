using ArticlesService.UnitOfWork.SQLite.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticlesService.UnitOfWork.SQLite.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddSQLite(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("AppContext");

        services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));

        return services;
    }
}