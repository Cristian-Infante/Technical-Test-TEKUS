using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class UpdateServiceCommandHandler(
    IServiceRepository repo,
    IUnitOfWork uow
) : IRequestHandler<UpdateServiceCommand>
{
    public async Task<Unit> Handle(UpdateServiceCommand cmd, CancellationToken ct)
    {
        var svc = await repo.GetByIdWithProvidersAsync(cmd.Id)
                  ?? throw new KeyNotFoundException($"Service {cmd.Id} not found.");
        svc.Name       = cmd.Name;
        svc.HourlyRate = cmd.HourlyRate;
        repo.Update(svc);
        uow.Save();
        return Unit.Value;
    }
}