using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class UpdateUserCommandHandler(IUserRepository repo, IUnitOfWork uow) : IRequestHandler<UpdateUserCommand>
{
    public Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = repo
            .Find(u => u.Id == command.Id)
            .SingleOrDefault();

        if (user is null)
            throw new KeyNotFoundException($"Usuario con Id '{command.Id}' no encontrado.");

        user.ChangeEmail(command.Email);

        repo.Update(user);
        uow.Save();

        return Task.FromResult(Unit.Value);
    }
}