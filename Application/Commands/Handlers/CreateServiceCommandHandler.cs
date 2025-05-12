using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class CreateServiceCommandHandler(
    IServiceRepository repo,
    IUnitOfWork uow
) : IRequestHandler<CreateServiceCommand, Guid>
{
    public async Task<Guid> Handle(CreateServiceCommand cmd, CancellationToken ct)
    {
        var svc = new Service {
            Id         = Guid.NewGuid(),
            Name       = cmd.Name,
            HourlyRate = cmd.HourlyRate
        };
        repo.Add(svc);
        uow.Save();
        return svc.Id;
    }
}