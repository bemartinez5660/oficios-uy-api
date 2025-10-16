using Microsoft.EntityFrameworkCore;
using OficiosUY.Api.Data;
using OficiosUY.Api.DTOs;

namespace OficiosUY.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            Phone = user.Phone,
            Email = user.Email,
            IsServiceProvider = user.IsServiceProvider
        };
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        user.FirstName = request.Name.Split(" ")[0];
        user.LastName = request.Name.Split(" ")[1];
        user.Email = request.Email;

        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            Phone = user.Phone,
            Email = user.Email,
            IsServiceProvider = user.IsServiceProvider
        };
    }
}
