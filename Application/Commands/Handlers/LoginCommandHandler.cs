using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepo,
    IPasswordHasher hasher,
    IJwtTokenService jwtService)
    : IRequestHandler<LoginCommand, AuthResultDto>
{
    public Task<AuthResultDto> Handle(LoginCommand cmd, CancellationToken cancellationToken)
    {
        var user = userRepo.GetByUserName(cmd.Username)
                   ?? throw new UnauthorizedAccessException("Username or password invalid");

        if (!hasher.Verify(user.PasswordHash!, cmd.Password))
            throw new UnauthorizedAccessException("Username or password invalid");

        var (token, expiresAt) = jwtService.GenerateToken(user);

        return Task.FromResult(new AuthResultDto(token, expiresAt));
    }
}