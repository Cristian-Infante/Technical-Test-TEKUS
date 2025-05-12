using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetUsersQueryHandler(IUserRepository repo) : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    public Task<PagedResult<UserDto>> Handle(GetUsersQuery q, CancellationToken ct)
    {
        var all = repo.GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Filter))
        {
            var f = q.Filter.ToLower();
            all = all.Where(u =>
                u.Username!.Contains(f, StringComparison.CurrentCultureIgnoreCase) ||
                u.Email!.Contains(f, StringComparison.CurrentCultureIgnoreCase)
            );
        }

        all = q.SortBy.ToLower() switch
        {
            "email"    => all.OrderBy(u => u.Email),
            "username" => all.OrderBy(u => u.Username),
            _          => all.OrderBy(u => u.Username)
        };

        var total = all.Count();
        var page  = all
            .Skip((q.PageNumber - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(u => new UserDto(u.Id, u.Username, u.Email))
            .ToList();

        var result = new PagedResult<UserDto>(page, total, q.PageNumber, q.PageSize);
        return Task.FromResult(result);
    }
}