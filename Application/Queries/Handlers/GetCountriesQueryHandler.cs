using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetCountriesQueryHandler(ICountryRepository svc)
    : IRequestHandler<GetCountriesQuery, PagedResult<CountryDto>>
{
    public async Task<PagedResult<CountryDto>> Handle(GetCountriesQuery q, CancellationToken ct)
    {
        var all = (await svc.GetAllIsoAndNameAsync()).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Filter))
        {
            var f = q.Filter.ToLower();
            all = all.Where(c =>
                c.Name!.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                c.IsoCode!.Contains(f, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        all = q.SortBy.ToLower() switch
        {
            "iso"  => all.OrderBy(c => c.IsoCode),
            "name" => all.OrderBy(c => c.Name),
            _      => all.OrderBy(c => c.Name)
        };

        var total = all.Count();
        var page  = all
            .Skip((q.PageNumber - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(c => new CountryDto(c.IsoCode, c.Name))
            .ToList();

        return new PagedResult<CountryDto>(page, total, q.PageNumber, q.PageSize);
    }
}