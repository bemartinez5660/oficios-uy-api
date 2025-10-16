namespace OficiosUY.Api.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ServiceId { get; set; }
    public DateTime DateRequested { get; set; }
    public string Status { get; set; } = "pending"; // pending, confirmed, completed, cancelled
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Service Service { get; set; } = null!;
}
