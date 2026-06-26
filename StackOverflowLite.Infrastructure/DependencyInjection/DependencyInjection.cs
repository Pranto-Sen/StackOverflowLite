using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Infrastructure.Services;

namespace StackOverflowLite.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)

    {
        services.AddHttpContextAccessor();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<ICurrentUserService,CurrentUserService>();

        services.AddScoped<IReputationService,ReputationService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration =
                configuration["Redis:ConnectionString"];
        });

        

        services.AddScoped<IRedisCacheService,RedisCacheService>();

        return services;
    }
}