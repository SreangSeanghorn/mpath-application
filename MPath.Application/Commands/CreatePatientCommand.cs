using System.Runtime.InteropServices.JavaScript;
using MPath.Application.ResponsesDTOs.Patients;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands;

public record CreatePatientCommand(
    string Name,
    string Email,
    string Phone,
    string Address,
    DateTime DateOfBirth,
    Guid UserId) : ICommand<PatientCreatedResponseDto>;


