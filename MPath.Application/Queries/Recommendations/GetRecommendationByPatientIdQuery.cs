using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Queries.Recommendations;

public class GetRecommendationByPatientIdQuery : IQuery<PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>
{
    public Guid PatientId { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public string OrderBy { get; init; }
    public string SortOrder { get; init; }
    public string FilterField { get; init; }
    public string FilterValue { get; init; }
}