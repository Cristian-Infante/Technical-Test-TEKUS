using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Commands;

public record LoginCommand(
    [Required] string Username,
    [Required] string Password
) : IRequest<AuthResultDto>;