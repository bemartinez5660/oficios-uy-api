using System.ComponentModel.DataAnnotations;

namespace OficiosUY.Api.DTOs;

public class UserDto
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
}
