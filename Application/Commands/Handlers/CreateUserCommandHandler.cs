using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class CreateUserCommandHandler(
    IUserRepository repo,
    IUnitOfWork uow,
    IPasswordHasher hasher)
    : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand c, CancellationToken _)
    {
        if (repo.GetByUserName(c.Username) != null)
            throw new InvalidOperationException("Usuario ya existe");

        var user = User.Create(c.Username, c.Password, c.Email, hasher);
        repo.Add(user);
        uow.Save();
        return user.Id;
    }
}