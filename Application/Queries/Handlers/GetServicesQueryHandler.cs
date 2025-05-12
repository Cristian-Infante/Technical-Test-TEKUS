using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetServicesQueryHandler(
    IServiceRepository repo
) : IRequestHandler<GetServicesQuery, PagedResult<ServiceDto>>
{
    public async Task<PagedResult<ServiceDto>> Handle(GetServicesQuery q, CancellationToken ct)
    {
        var all = repo.GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Filter))
        {
            var f = q.Filter.ToLower();
            all = all.Where(s =>
                s.Name!.Contains(f, StringComparison.CurrentCultureIgnoreCase));
        }

        all = q.SortBy.ToLower() switch
        {
            "hourlyrate" => all.OrderBy(s => s.HourlyRate),
            "name"       => all.OrderBy(s => s.Name),
            _            => all.OrderBy(s => s.Name)
        };

        var total = all.Count();
        var page  = all
            .Skip((q.PageNumber - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(s => new ServiceDto(s.Id, s.Name!, s.HourlyRate))
            .ToList();

        return new PagedResult<ServiceDto>(page, total, q.PageNumber, q.PageSize);
    }
}