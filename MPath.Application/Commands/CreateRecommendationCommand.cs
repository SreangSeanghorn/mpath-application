

using MPath.Application.ResponsesDTOs.Recommendations;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands;

public record CreateRecommendationCommand(
    string Title,
    string Content,
    Guid PatientId,
    Guid UserId
) : ICommand<CreateRecommendationCommandResponseDto>
{
    
}