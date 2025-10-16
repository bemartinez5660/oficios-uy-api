namespace OficiosUY.Api.DTOs;

public class ServiceDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string PriceRange { get; set; } = string.Empty;
    public int ProviderId { get; set; }
    public string ProviderName { get; set; } = string.Empty;
}

public class CreateServiceRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string PriceRange { get; set; } = string.Empty;
}
