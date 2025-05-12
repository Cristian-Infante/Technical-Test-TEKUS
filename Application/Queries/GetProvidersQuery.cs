using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetProvidersQuery(
    int PageNumber = 1,
    int PageSize   = 10,
    string SortBy  = "name",
    string? Filter = null
) : IRequest<PagedResult<ProviderDto>>;