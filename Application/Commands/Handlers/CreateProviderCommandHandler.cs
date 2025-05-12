using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class CreateProviderCommandHandler(
    IProviderRepository repo,
    IUnitOfWork uow,
    IRepository<Service> serviceRepo
) : IRequestHandler<CreateProviderCommand, Guid>
{
    public async Task<Guid> Handle(CreateProviderCommand cmd, CancellationToken ct)
    {
        var provider = Provider.Create(cmd.Nit, cmd.Name, cmd.Email);

        if (cmd.CustomFields != null)
            foreach (var cf in cmd.CustomFields)
                provider.AddOrUpdateCustomField(cf.Key, cf.Value);

        if (cmd.ServicesOffered != null)
        {
            foreach (var so in cmd.ServicesOffered)
            {
                var svc = serviceRepo.GetById(so.ServiceId);
                if (svc == null) throw new KeyNotFoundException($"Service {so.ServiceId} no existe");
                provider.OfferService(svc, so.CountryIsoCodes);
            }
        }

        repo.Add(provider);
        uow.Save();
        return provider.Id;
    }
}