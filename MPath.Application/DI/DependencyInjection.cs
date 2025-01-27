using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MPath.Application.CommandHandlers;
using MPath.Application.Commands;
using MPath.Application.Queries.Patients;
using MPath.Application.QueriesHandlers.Patients;
using MPath.Application.ResponsesDTOs;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.Application.Validators;
using MPath.Application.Validators.Patients;
using MPath.Application.Validators.UserLogin;
using MPath.Application.Validators.UserRegistered;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<UserRegisterCommand, UserRegisteredResponse>, UserRegisteredCommandHandler>();
        services.AddScoped<IValidator<UserRegisterCommand>, UserRegisteredCommandValidator>();
        services.AddScoped<ICommandHandler<UserLoginCommand, UserLoginResponseDto>, UserLoginCommandHandler>();
        services.AddScoped<IValidator<UserLoginCommand>, UserLoginCommandValidator>();
        services
            .AddScoped<IQueryHandler<GetListOfPatientsQuery, PaginationDto<IEnumerable<GetListOfPatientResponseDto>>>,
                GetListOfPatientsQueryHandler>();
        services.AddScoped<IValidator<CreatePatientCommand>, CreatePatientCommandValidator>();
        services.AddScoped<ICommandHandler<CreatePatientCommand, PatientCreatedResponseDto>,
            CreatePatientCommandHandler>();
        services.AddScoped<IValidator<GetListOfPatientsQuery>, GetListOfPatientQueryValidator>();
        return services;
    }
}