using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetProviderByIdQuery(Guid Id) : IRequest<ProviderDto?>;