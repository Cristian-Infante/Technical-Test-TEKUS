namespace Domain.Entities;

public class Country
{
    public string? IsoCode { get; set; }
    public string? Name { get; set; }

    public ICollection<ProviderServiceCountry> ProviderServiceCountries { get; set; } = new List<ProviderServiceCountry>();
}