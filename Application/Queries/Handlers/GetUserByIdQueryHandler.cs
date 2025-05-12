using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetUserByIdQueryHandler(IUserRepository repo) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery q, CancellationToken ct)
    {
        var user = await repo.GetAll().ToAsyncEnumerable()
            .FirstOrDefaultAsync(u => u.Id == q.Id, cancellationToken: ct);

        return user is null
            ? null
            : new UserDto(user.Id, user.Username, user.Email);
    }
}