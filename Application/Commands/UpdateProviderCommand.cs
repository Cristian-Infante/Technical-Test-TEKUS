using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record UpdateProviderCommand(
    [Required] Guid Id,
    [Required] string Nit,
    [Required] string Name,
    [Required, EmailAddress] string Email,
    IEnumerable<CustomFieldDto>? CustomFields,
    IEnumerable<ServiceOfferInputDto>? ServicesOffered
) : IRequest;