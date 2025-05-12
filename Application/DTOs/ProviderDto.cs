namespace Application.DTOs;

public record ProviderDto(
    Guid Id,
    string? Nit,
    string? Name,
    string? Email,
    IEnumerable<CustomFieldDto> CustomFields,
    IEnumerable<ServiceOfferDto> ServicesOffered
);