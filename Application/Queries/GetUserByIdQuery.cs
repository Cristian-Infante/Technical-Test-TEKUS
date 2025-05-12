using Application.DTOs;
using MediatR;

namespace Application.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;