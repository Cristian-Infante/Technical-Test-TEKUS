namespace Domain.Interfaces;

public interface IPasswordHasher
{
    string Hash(string plain);
    bool   Verify(string hash, string plain);
}