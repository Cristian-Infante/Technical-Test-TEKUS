using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class DeleteServiceCommandHandler(
    IServiceRepository repo,
    IUnitOfWork uow
) : IRequestHandler<DeleteServiceCommand>
{
    public async Task<Unit> Handle(DeleteServiceCommand cmd, CancellationToken ct)
    {
        var svc = await repo.GetByIdWithProvidersAsync(cmd.Id)
                  ?? throw new KeyNotFoundException($"Service {cmd.Id} not found.");
        repo.Delete(svc);
        uow.Save();
        return Unit.Value;
    }
}