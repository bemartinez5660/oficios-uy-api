using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosUY.Api.DTOs;
using OficiosUY.Api.Services;
using System.Security.Claims;

namespace OficiosUY.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { success = false, message = "Invalid token" });

        var booking = await _bookingService.CreateBookingAsync(userId, request);
        if (booking == null)
            return BadRequest(new { success = false, message = "Failed to create booking" });

        return CreatedAtAction(nameof(GetUserBookings), new { userId }, new { success = true, booking });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserBookings(int userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var currentUserId))
            return Unauthorized(new { success = false, message = "Invalid token" });

        // Users can only view their own bookings
        if (currentUserId != userId)
            return Forbid();

        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(new { success = true, bookings });
    }
}
