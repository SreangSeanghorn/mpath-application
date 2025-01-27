using MPath.Application.Queries.Recommendations;
using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.Application.Shared.Responses;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.QueriesHandlers.Recommendations;

public class GetRecommendationByPatientIdQueryHandler : IQueryHandler<GetRecommendationByPatientIdQuery,PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>>
{
    private readonly IRecommendationRepository _recommendationRepository;
    public Task<PaginationDto<IEnumerable<GetRecommendationByPatientIdResponseDto>>> Handle(GetRecommendationByPatientIdQuery query)
    {
        throw new NotImplementedException();
    }
}