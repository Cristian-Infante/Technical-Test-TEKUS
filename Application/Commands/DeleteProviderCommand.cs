using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands;

public record DeleteProviderCommand(
    [Required] Guid Id
) : IRequest;