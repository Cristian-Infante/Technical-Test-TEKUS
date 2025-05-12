namespace Domain.Entities;

public class Provider
{
    public Guid Id { get; set; }
    public string? Nit { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();
    public ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();
    
    private Provider() { }

    public static Provider Create(string nit, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(nit)) throw new ArgumentException("NIT requerido");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name requerido");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email requerido");

        return new Provider {
            Id    = Guid.NewGuid(),
            Nit   = nit,
            Name  = name,
            Email = email
        };
    }

    public void ChangeInfo(string nit, string name, string email)
    {
        if (string.IsNullOrWhiteSpace(nit)) throw new ArgumentException("NIT inválido");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name inválido");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email inválido");

        Nit   = nit;
        Name  = name;
        Email = email;
    }

    public void AddOrUpdateCustomField(string key, string value)
    {
        var cf = CustomFields.SingleOrDefault(x => x.Key == key);
        if (cf != null)
            cf.Value = value;
        else
            CustomFields.Add(new CustomField { Id = Guid.NewGuid(), Key = key, Value = value, ProviderId = Id });
    }

    public void RemoveCustomField(Guid customFieldId)
    {
        var cf = CustomFields.SingleOrDefault(x => x.Id == customFieldId);
        if (cf != null) CustomFields.Remove(cf);
    }

    public void OfferService(Service service, IEnumerable<string> countryIsoCodes)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        var ps = new ProviderService {
            ProviderServiceId = Guid.NewGuid(),
            ProviderId        = Id,
            ServiceId         = service.Id
        };

        foreach (var iso in countryIsoCodes.Distinct())
        {
            ps.ServiceCountries.Add(new ProviderServiceCountry {
                ProviderServiceId = ps.ProviderServiceId,
                CountryIsoCode    = iso
            });
        }

        ProviderServices.Add(ps);
    }

    public void RemoveServiceOffer(Guid providerServiceId)
    {
        var ps = ProviderServices.SingleOrDefault(x => x.ProviderServiceId == providerServiceId);
        if (ps != null) ProviderServices.Remove(ps);
    }
}