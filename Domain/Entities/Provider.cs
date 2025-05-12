namespace Domain.Entities;

public class Provider
{
    public Guid Id { get; set; }
    public string? Nit { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    
    public ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();
    public ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();
}