using FluentValidation;
using MPath.Domain.Repositories;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands;

public record MarkRecommendationAsCompletedCommand(Guid RecommendationId,Guid UserId) : ICommand<bool>
{
    
}