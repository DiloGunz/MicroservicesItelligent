using ArticlesService.Repository.Abstraction;
using ArticlesService.Repository.SQLite;
using ArticlesService.UnitOfWork.Abstraction;
using ArticlesService.UnitOfWork.SQLite.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticlesService.UnitOfWork.Repository.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration config)
    {
        services.AddSQLite(config);
        services.AddScoped<IUnitOfWork, UnitOfWorkContainer>();
        services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();

        services.AddTransient<IArticleRepository, ArticleRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();

        return services;
    }
}