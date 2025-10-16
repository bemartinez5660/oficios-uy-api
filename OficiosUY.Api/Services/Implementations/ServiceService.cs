using Microsoft.EntityFrameworkCore;
using OficiosUY.Api.Data;
using OficiosUY.Api.DTOs;
using OficiosUY.Api.Entities;

namespace OficiosUY.Api.Services.Implementations;

public class ServiceService : IServiceService
{
    private readonly ApplicationDbContext _context;

    public ServiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        return await _context.Services
            .Include(s => s.Provider)
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                Category = s.Category,
                PriceRange = s.PriceRange,
                ProviderId = s.ProviderId,
                ProviderName = s.Provider.FirstName
            })
            .ToListAsync();
    }

    public async Task<ServiceDto?> CreateServiceAsync(int providerId, CreateServiceRequest request)
    {
        var provider = await _context.Users.FindAsync(providerId);
        if (provider == null || provider.IsServiceProvider) return null;

        var service = new Service
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            PriceRange = request.PriceRange,
            ProviderId = providerId
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return new ServiceDto
        {
            Id = service.Id,
            Title = service.Title,
            Description = service.Description,
            Category = service.Category,
            PriceRange = service.PriceRange,
            ProviderId = service.ProviderId,
            ProviderName = provider.FirstName
        };
    }
}
