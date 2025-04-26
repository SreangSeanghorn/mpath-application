namespace MPath.Application.ResponsesDTOs;

public record UserRefreshTokenResponseDto(
    string AccessToken,
    string RefreshToken
    );