using System.Security.Cryptography;
using System.Text;

namespace ClickStitch.Api.Auth;

public interface IPasswordService
{
    string Hash(string password, Guid salt);
    bool IsMatch(string expectedPassword, string password, Guid salt);
}

public sealed class PasswordService : IPasswordService
{
    private const string PEPPER = "f11f9f6c-7ed6-4407-bc91-515b0cb7b25b";

    public string Hash(string password, Guid salt)
    {
        var toHash = password + salt + PEPPER;

        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(toHash)));
    }

    public bool IsMatch(string expectedPassword, string password, Guid salt)
    {
        var hashed = Hash(password, salt);

        return hashed == expectedPassword;
    }
}