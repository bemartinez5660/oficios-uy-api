using Microsoft.EntityFrameworkCore;
using OficiosUY.Api.Configuration;
using OficiosUY.Api.Data;
using OficiosUY.Api.DTOs;
using OficiosUY.Api.DTOs.Auth;
using OficiosUY.Api.Entities;
using BCrypt.Net;

namespace OficiosUY.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtService _jwtService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(ApplicationDbContext context, JwtService jwtService, JwtSettings jwtSettings)
    {
        _context = context;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings;
    }

    public async Task<(bool Success, string Message, UserDto? User)> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return (false, "Email already exists", null);

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            IsServiceProvider = request.IsServiceProvider,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            IsServiceProvider = request.IsServiceProvider,
        };

        return (true, "Registration successful", userDto);
    }

    public async Task<(bool Success, string Message, UserDto? User, string AccessToken, string RefreshToken)> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return (false, "Invalid credentials", null, string.Empty, string.Empty);

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = request.Email,
            Phone = request.Password
        };

        return (true, "Login successful", userDto, accessToken, refreshToken);
    }

    public async Task<(bool Success, string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            return (false, string.Empty, string.Empty);

        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);
        await _context.SaveChangesAsync();

        return (true, newAccessToken, newRefreshToken);
    }

    public async Task<UserDto?> GetCurrentUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            IsServiceProvider = user.IsServiceProvider,
        };
    }
}
