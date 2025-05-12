using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetCountryByIsoQueryHandler(ICountryRepository repo) : IRequestHandler<GetCountryByIsoQuery, CountryDto?>
{
    public async Task<CountryDto?> Handle(GetCountryByIsoQuery q, CancellationToken _)
    {
        var c = await repo.GetByIsoAsync(q.IsoCode);
        return c is null
            ? null
            : new CountryDto(c.IsoCode!, c.Name!);
    }
}