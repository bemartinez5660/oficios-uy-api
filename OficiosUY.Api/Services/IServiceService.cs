using OficiosUY.Api.DTOs;

namespace OficiosUY.Api.Services;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto?> CreateServiceAsync(int providerId, CreateServiceRequest request);
}
