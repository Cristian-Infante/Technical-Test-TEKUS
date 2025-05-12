using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class DeleteUserCommandHandler(IUserRepository repo, IUnitOfWork uow) : IRequestHandler<DeleteUserCommand>
{
    public Task<Unit> Handle(DeleteUserCommand c, CancellationToken _)
    {
        var user = repo.GetByUserName(c.Id.ToString());
        if (user is null) throw new KeyNotFoundException();
        repo.Delete(user);
        uow.Save();
        return Task.FromResult(Unit.Value);
    }
}