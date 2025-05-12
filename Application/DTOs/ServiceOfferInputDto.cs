using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ServiceOfferInputDto(
    [Required] Guid ServiceId,
    [Required] IEnumerable<string> CountryIsoCodes
);