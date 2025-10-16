using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosUY.Api.Configuration;
using OficiosUY.Api.DTOs.Auth;
using OficiosUY.Api.Services;
using System.Security.Claims;

namespace OficiosUY.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IAuthService authService, JwtSettings jwtSettings)
    {
        _authService = authService;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (success, message, user) = await _authService.RegisterAsync(request);

        if (!success)
            return BadRequest(new AuthResponse { Success = false, Message = message });

        // Auto-login after registration
        var loginRequest = new LoginRequest { Email = request.Email, Password = request.Password };
        var (loginSuccess, _, loginUser, accessToken, refreshToken) = await _authService.LoginAsync(loginRequest);

        if (loginSuccess)
        {
            SetTokenCookies(accessToken, refreshToken);
        }

        return Ok(new AuthResponse { Success = true, Message = message, User = user });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (success, message, user, accessToken, refreshToken) = await _authService.LoginAsync(request);

        if (!success)
            return Unauthorized(new AuthResponse { Success = false, Message = message });

        SetTokenCookies(accessToken, refreshToken);

        return Ok(new AuthResponse { Success = true, Message = message, User = user });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        return Ok(new { success = true, message = "Logged out successfully" });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { success = false, message = "Refresh token not found" });

        var (success, accessToken, newRefreshToken) = await _authService.RefreshTokenAsync(refreshToken);

        if (!success)
            return Unauthorized(new { success = false, message = "Invalid refresh token" });

        SetTokenCookies(accessToken, newRefreshToken);

        return Ok(new { success = true, message = "Token refreshed" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { success = false, message = "Invalid token" });

        var user = await _authService.GetCurrentUserAsync(userId);
        if (user == null)
            return NotFound(new { success = false, message = "User not found" });

        return Ok(new { success = true, user });
    }

    private void SetTokenCookies(string accessToken, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };

        Response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = cookieOptions.HttpOnly,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Path = cookieOptions.Path,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes)
        });

        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = cookieOptions.HttpOnly,
            Secure = cookieOptions.Secure,
            SameSite = cookieOptions.SameSite,
            Path = cookieOptions.Path,
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
    }
}
