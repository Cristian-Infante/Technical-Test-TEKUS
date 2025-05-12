using Domain.Entities;

namespace Domain.Interfaces;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllIsoAndNameAsync();
    Task<Country?> GetByIsoAsync(string isoCode);
}