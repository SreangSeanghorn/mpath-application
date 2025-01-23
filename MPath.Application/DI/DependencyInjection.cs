using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.Application.Validators;
using MPath.Application.Validators.UserRegistered;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<UserRegisterCommand, BaseResponse<UserRegisteredResponse>>,
                UserRegisteredCommandHandler>();
        services.AddScoped<IValidator<UserRegisterCommand>, UserRegisteredCommandValidator>();
        return services;
    }
}