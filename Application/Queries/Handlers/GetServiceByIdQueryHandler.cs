using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetServiceByIdQueryHandler(
    IServiceRepository repo
) : IRequestHandler<GetServiceByIdQuery, ServiceDto?>
{
    public async Task<ServiceDto?> Handle(GetServiceByIdQuery q, CancellationToken ct)
    {
        var s = await repo.GetByIdWithProvidersAsync(q.Id);
        return s is null
            ? null
            : new ServiceDto(s.Id, s.Name!, s.HourlyRate);
    }
}