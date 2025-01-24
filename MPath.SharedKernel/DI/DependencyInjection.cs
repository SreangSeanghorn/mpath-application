using Microsoft.Extensions.DependencyInjection;
using MPath.SharedKernel.Event;
using MPath.SharedKernel.Primitive;

namespace MPath.SharedKernel.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddScoped<ICommandResolver, CommandResolver>();
        services.AddScoped<IQueryResolver, QueryResolver>();
        return services;
    }
}