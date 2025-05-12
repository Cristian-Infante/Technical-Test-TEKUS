using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetServiceByIdQuery(Guid Id) : IRequest<ServiceDto?>;