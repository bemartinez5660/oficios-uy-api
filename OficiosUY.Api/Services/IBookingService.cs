using OficiosUY.Api.DTOs;

namespace OficiosUY.Api.Services;

public interface IBookingService
{
    Task<BookingDto?> CreateBookingAsync(int userId, CreateBookingRequest request);
    Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId);
}
