using Microsoft.EntityFrameworkCore;
using OficiosUY.Api.Data;
using OficiosUY.Api.DTOs;
using OficiosUY.Api.Entities;

namespace OficiosUY.Api.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookingDto?> CreateBookingAsync(int userId, CreateBookingRequest request)
    {
        var service = await _context.Services.FindAsync(request.ServiceId);
        if (service == null) return null;

        var booking = new Booking
        {
            UserId = userId,
            ServiceId = request.ServiceId,
            DateRequested = request.DateRequested,
            Status = "pending"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            ServiceId = booking.ServiceId,
            ServiceTitle = service.Title,
            DateRequested = booking.DateRequested,
            Status = booking.Status
        };
    }

    public async Task<IEnumerable<BookingDto>> GetUserBookingsAsync(int userId)
    {
        return await _context.Bookings
            .Include(b => b.Service)
            .Where(b => b.UserId == userId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                ServiceId = b.ServiceId,
                ServiceTitle = b.Service.Title,
                DateRequested = b.DateRequested,
                Status = b.Status
            })
            .ToListAsync();
    }
}
