namespace Application.DTOs;


public record AuthResultDto(
    string AccessToken,
    DateTime ExpiresAt
);