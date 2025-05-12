using Application.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(MetricsDto), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        var totalProviders = uow.Providers.GetAll().Count();
        var totalServices  = uow.Services.GetAll().Count();

        return Ok(new MetricsDto(totalProviders, totalServices));
    }
}