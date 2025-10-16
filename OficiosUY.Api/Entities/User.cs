using System.ComponentModel.DataAnnotations;

namespace OficiosUY.Api.Entities;

public class User
{
    public int Id { get; set; }
    
    [MaxLength(50)] 
    public string FirstName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(50)] 
    public string Phone { get; set; } = string.Empty;

    public bool IsServiceProvider { get; set; } = false;
    
    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}