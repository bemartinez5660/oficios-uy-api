namespace OficiosUY.Api.DTOs.Auth;

public class RegisterRequest
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public bool IsServiceProvider { get; set; } = false;
}
