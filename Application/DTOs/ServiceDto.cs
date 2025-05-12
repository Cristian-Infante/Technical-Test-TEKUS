namespace Application.DTOs;

public record ServiceDto(
    Guid Id,
    string Name,
    decimal HourlyRate
);