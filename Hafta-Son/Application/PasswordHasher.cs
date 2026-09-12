using System.Security.Cryptography;
namespace HangarDesk.Final.Application;
internal sealed class PasswordHasher
{
    private const int Iterations = 120000;
    public (byte[] Hash, byte[] Salt) Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(32);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return (hash, salt);
    }
    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        byte[] candidate = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(candidate, hash);
    }
}
