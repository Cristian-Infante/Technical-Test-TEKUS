using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands;

public record UpdateUserCommand(
    Guid Id,
    [Required, EmailAddress] string Email
) : IRequest;