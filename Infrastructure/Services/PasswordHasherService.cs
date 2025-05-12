using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object?> _hasher = new();
    public string Hash(string plain) => _hasher.HashPassword(null, plain);
    public bool   Verify(string hash, string plain) =>
        _hasher.VerifyHashedPassword(null, hash, plain) != PasswordVerificationResult.Failed;
}