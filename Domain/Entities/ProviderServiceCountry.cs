namespace Domain.Entities;

public class ProviderServiceCountry
{
    public Guid ProviderServiceId { get; set; }
    public string? CountryIsoCode { get; set; }

    public ProviderService? ProviderService { get; set; }
    public Country? Country { get; set; }
}