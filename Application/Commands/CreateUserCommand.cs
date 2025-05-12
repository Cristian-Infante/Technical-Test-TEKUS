using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands;

public record CreateUserCommand(
    [Required, MinLength(3)] string Username,
    [Required, MinLength(6)] string Password,
    [Required, EmailAddress] string Email
) : IRequest<Guid>;