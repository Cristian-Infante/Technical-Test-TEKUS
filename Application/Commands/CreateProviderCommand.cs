using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record CreateProviderCommand(
    [Required] string Nit,
    [Required] string Name,
    [Required, EmailAddress] string Email,
    IEnumerable<CustomFieldDto>? CustomFields,
    IEnumerable<ServiceOfferInputDto>? ServicesOffered
) : IRequest<Guid>;