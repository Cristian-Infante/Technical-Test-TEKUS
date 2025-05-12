using Domain.Interfaces;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }

    private User() { }

    public static User Create(string username, string plainPassword, string email, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(username)) 
            throw new ArgumentException("Username requerido");
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("Password requerido");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email requerido");

        return new User {
            Id           = Guid.NewGuid(),
            Username     = username,
            PasswordHash = hasher.Hash(plainPassword),
            Email        = email
        };
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail)) 
            throw new ArgumentException("Email inválido");
        Email = newEmail;
    }

    public void ChangePassword(string newPlain, IPasswordHasher hasher)
    {
        if (string.IsNullOrWhiteSpace(newPlain)) 
            throw new ArgumentException("Password inválido");
        PasswordHash = hasher.Hash(newPlain);
    }
}