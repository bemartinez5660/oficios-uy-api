using OficiosUY.Api.DTOs;
using OficiosUY.Api.DTOs.Auth;

namespace OficiosUY.Api.Services;

public interface IAuthService
{
    Task<(bool Success, string Message, UserDto? User)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, string Message, UserDto? User, string AccessToken, string RefreshToken)> LoginAsync(LoginRequest request);
    Task<(bool Success, string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
    Task<UserDto?> GetCurrentUserAsync(int userId);
}
