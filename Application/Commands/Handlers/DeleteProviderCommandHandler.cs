using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class DeleteProviderCommandHandler(
    IProviderRepository repo,
    IUnitOfWork uow
) : IRequestHandler<DeleteProviderCommand>
{
    public async Task<Unit> Handle(DeleteProviderCommand cmd, CancellationToken ct)
    {
        var provider = await repo.GetByIdWithServicesAsync(cmd.Id);
        if (provider is null)
            throw new KeyNotFoundException($"Provider with ID {cmd.Id} not found.");

        repo.Delete(provider);
        uow.Save();
        return Unit.Value;
    }
}