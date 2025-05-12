using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands;

public record DeleteServiceCommand(
    [Required] Guid Id
) : IRequest;