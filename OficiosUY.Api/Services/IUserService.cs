using OficiosUY.Api.DTOs;

namespace OficiosUY.Api.Services;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request);
}
