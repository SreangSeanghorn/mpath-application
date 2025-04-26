using MPath.Application.ResponsesDTOs;
using MPath.Application.Shared.Responses;
using MPath.SharedKernel.Primitive;

namespace MPath.Application.Commands;

public record UserRefreshTokenCommand(
    string RefreshToken
): ICommand<UserRefreshTokenResponseDto>
{
    
}