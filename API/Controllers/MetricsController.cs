// API/Controllers/MetricsController.cs
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetricsController(IUnitOfWork uow) : ControllerBase
{
}