namespace Domain.Entities;

public class CustomField
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public string? Key { get; set; }
    public string? Value { get; set; }

    public Provider? Provider { get; set; }
}