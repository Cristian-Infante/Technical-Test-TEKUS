using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CountriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CountryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize   = 10,
        [FromQuery] string sortBy  = "name",
        [FromQuery] string? filter = null
    ) {
        var result = await mediator.Send(
            new GetCountriesQuery(pageNumber, pageSize, sortBy, filter)
        );
        return Ok(result);
    }
    
    [HttpGet("{iso}")]
    [ProducesResponseType(typeof(CountryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByIso(string iso)
    {
        var country = await mediator.Send(new GetCountryByIsoQuery(iso));
        return country is null
            ? NotFound()
            : Ok(country);
    }
}