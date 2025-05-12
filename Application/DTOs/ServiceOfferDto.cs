namespace Application.DTOs;

public record ServiceOfferDto(
    Guid ServiceId,
    string ServiceName,
    decimal HourlyRate,
    IEnumerable<string> CountryIsoCodes
);