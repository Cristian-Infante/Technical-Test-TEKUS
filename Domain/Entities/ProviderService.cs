namespace Domain.Entities;

public class ProviderService
{
    public Guid ProviderServiceId { get; set; }
    public Guid ProviderId { get; set; }
    public Guid ServiceId { get; set; }

    public Provider? Provider { get; set; }
    public Service? Service  { get; set; }

    public ICollection<ProviderServiceCountry> ServiceCountries { get; set; } = new List<ProviderServiceCountry>();
}