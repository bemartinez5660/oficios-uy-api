namespace OficiosUY.Api.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ServiceId { get; set; }
    public string ServiceTitle { get; set; } = string.Empty;
    public DateTime DateRequested { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateBookingRequest
{
    public int ServiceId { get; set; }
    public DateTime DateRequested { get; set; }
}
