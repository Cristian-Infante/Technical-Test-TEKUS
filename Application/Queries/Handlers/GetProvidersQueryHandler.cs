using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetProvidersQueryHandler(
    IProviderRepository repo
) : IRequestHandler<GetProvidersQuery, PagedResult<ProviderDto>>
{
    public async Task<PagedResult<ProviderDto>> Handle(GetProvidersQuery q, CancellationToken ct)
    {
        var all = (await repo.GetAllWithServicesAsync()).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Filter))
        {
            var f = q.Filter.ToLower();
            all = all.Where(p =>
                p.Name!.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                p.Nit!.Contains(f, StringComparison.CurrentCultureIgnoreCase));
        }

        all = q.SortBy.ToLower() switch
        {
            "nit"  => all.OrderBy(p => p.Nit),
            "name" => all.OrderBy(p => p.Name),
            _      => all.OrderBy(p => p.Name)
        };

        var total = all.Count();
        var page  = all
            .Skip((q.PageNumber - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(p => new ProviderDto(
                p.Id,
                p.Nit,
                p.Name,
                p.Email,
                p.CustomFields.Select(cf => new CustomFieldDto(cf.Id, cf.Key!, cf.Value!)),
                p.ProviderServices.Select(ps => new ServiceOfferDto(
                    ps.ServiceId,
                    ps.Service!.Name!,
                    ps.Service.HourlyRate,
                    ps.ServiceCountries.Select(c => c.CountryIsoCode!)
                ))
            ))
            .ToList();

        return new PagedResult<ProviderDto>(page, total, q.PageNumber, q.PageSize);
    }
}