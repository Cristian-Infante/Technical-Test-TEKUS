using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands;

public record CreateServiceCommand(
    [Required, MinLength(3)] string Name,
    [Required, Range(0.01, double.MaxValue)] decimal HourlyRate
) : IRequest<Guid>;