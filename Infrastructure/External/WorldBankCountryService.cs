using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.External;

public class WorldBankCountryService(HttpClient http) : ICountryRepository
{
    public async Task<IEnumerable<Country>> GetAllIsoAndNameAsync()
    {
        var json = await http.GetStringAsync(
            "https://api.worldbank.org/v2/country?format=json");
        using var doc = JsonDocument.Parse(json);
        var items = doc.RootElement[1]
            .EnumerateArray()
            .Select(e => new Country {
                IsoCode = e.GetProperty("id").GetString()!,
                Name    = e.GetProperty("name").GetString()!
            })
            .ToList();
        return items;
    }
    
    public async Task<Country?> GetByIsoAsync(string isoCode)
    {
        var json = await http.GetStringAsync(
            $"https://api.worldbank.org/v2/country/{isoCode}?format=json"
        );
        using var doc = JsonDocument.Parse(json);
        var arr = doc.RootElement[1].EnumerateArray();
        if (!arr.Any()) return null;
        var e = arr.First();
        return new Country {
            IsoCode = e.GetProperty("id").GetString()!,
            Name    = e.GetProperty("name").GetString()!
        };
    }
}