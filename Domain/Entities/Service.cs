namespace Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal HourlyRate { get; set; }

    public ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();
}