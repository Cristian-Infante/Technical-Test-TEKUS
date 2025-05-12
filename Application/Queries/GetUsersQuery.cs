using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetUsersQuery(
    int PageNumber = 1,
    int PageSize   = 10,
    string SortBy  = "username",
    string? Filter = null
) : IRequest<PagedResult<UserDto>>;