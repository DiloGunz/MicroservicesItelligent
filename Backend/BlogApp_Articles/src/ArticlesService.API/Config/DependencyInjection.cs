using ArticlesService.API.Utils;
using ArticlesService.API.Utils.Middlewares;
using ArticlesService.Application.Config;
using ArticlesService.SharedKernel.ContextAccessor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ArticlesService.API.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddTransient<GloblalExceptionHandlingMiddleware>();

        services.AddControllers(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });

        services.AddApplication(configuration);

        var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Jwt:Key") ?? string.Empty);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
        {
            x.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                    if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                    else if (authHeader != null)
                    {
                        context.Token = authHeader.Trim();
                    }
                    return Task.CompletedTask;
                }
            };
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        AddAuth(services);

        services.AddHttpContextAccessor();

        services.AddScoped<IHttpContextAccesorProvider, HttpContextAccesorProvider>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddControllers(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                             .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });

        return services;
    }
}