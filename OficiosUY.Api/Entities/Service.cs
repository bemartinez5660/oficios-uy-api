namespace OficiosUY.Api.Entities;

public class Service
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string PriceRange { get; set; } = string.Empty;
    public int ProviderId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public User Provider { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
