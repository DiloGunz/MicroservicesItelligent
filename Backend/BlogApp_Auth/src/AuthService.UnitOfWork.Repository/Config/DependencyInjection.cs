using AuthService.Repository.Abstraction;
using AuthService.Repository.SQLite;
using AuthService.UnitOfWork.Abstraction;
using AuthService.UnitOfWork.SQLite.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.UnitOfWork.Repository.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration config)
    {
        services.AddSQLite(config);
        services.AddScoped<IUnitOfWork, UnitOfWorkContainer>();
        services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
        services.AddTransient<IAppUserRepository, AppUserRepository>();
        return services;
    }
}