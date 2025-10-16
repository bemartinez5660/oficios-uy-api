using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficiosUY.Api.DTOs;
using OficiosUY.Api.Services;
using System.Security.Claims;

namespace OficiosUY.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return Ok(new { success = true, services });
    }

    [Authorize(Roles = "provider")]
    [HttpPost]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { success = false, message = "Invalid token" });

        var service = await _serviceService.CreateServiceAsync(userId, request);
        if (service == null)
            return BadRequest(new { success = false, message = "Failed to create service" });

        return CreatedAtAction(nameof(GetAllServices), new { id = service.Id }, new { success = true, service });
    }
}
