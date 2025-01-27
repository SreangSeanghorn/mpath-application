using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Queries.Recommendations;

public record GetRecommendationByPatientIdQuery : IQuery<PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>
{
    public Guid PatientId { get; init; }
}