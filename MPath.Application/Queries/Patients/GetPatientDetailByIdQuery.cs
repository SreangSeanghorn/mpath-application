using MPath.Application.ResponsesDTOs.Patients;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Queries.Patients;

public record GetPatientDetailByIdQuery (Guid Id): IQuery<PatientDetailResponseDto>
{
}