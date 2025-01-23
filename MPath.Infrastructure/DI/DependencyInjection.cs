using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MPath.Domain;
using MPath.Domain.Core.Interfaces;
using MPath.Domain.Repositories;
using MPath.Infrastructure.Authentication;
using MPath.Infrastructure.Authentication.JwtRefreshTokenGenerator;
using MPath.Infrastructure.Authentication.JwtTokenGenerator;
using MPath.Infrastructure.MessageBroker;
using MPath.Infrastructure.Persistence.DBContext;
using MPath.Infrastructure.Repositories;
using MPath.Infrastructure.Security;
using MPath.SharedKernel.Event;

namespace MPath.Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IJwtRefreshTokenGenerator, JwtRefreshTokenGenerator>();
        services.AddScoped<IEventPublisher, InMemoryEventPublisher>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        return services;
    }
}