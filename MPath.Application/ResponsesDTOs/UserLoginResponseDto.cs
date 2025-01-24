namespace MPath.Application.ResponsesDTOs;

public record UserLoginResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresIn);