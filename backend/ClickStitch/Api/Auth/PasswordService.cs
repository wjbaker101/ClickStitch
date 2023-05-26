using Core.Settings;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ClickStitch.Api.Auth;

public interface IPasswordService
{
    string Hash(string password, string salt);
    bool IsMatch(string expectedPassword, string password, string salt);
    Result IsValid(string password);
}

public sealed partial class PasswordService : IPasswordService
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex HasNumberRegex();

    [GeneratedRegex("[A-Z]+")]
    private static partial Regex HasUpperCaseRegex();

    [GeneratedRegex("[a-z]+")]
    private static partial Regex HasLowerCaseRegex();

    [GeneratedRegex("[!@#$%^&*()_+=\\[{\\]};:<>|./?,-]")]
    private static partial Regex HasSymbolRegex();

    private readonly string _pepper;

    public PasswordService(AppSecrets secrets)
    {
        _pepper = secrets.Auth.Password.Pepper;
    }

    public string Hash(string password, string salt)
    {
        var toHash = password + salt + _pepper;

        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(toHash)));
    }

    public bool IsMatch(string expectedPassword, string password, string salt)
    {
        var hashed = Hash(password, salt);

        return hashed == expectedPassword;
    }

    public Result IsValid(string password)
    {
        const string symbols = "!@#$%^&*()_+=\\[{\\]};:<>|./?,-";

        const string requirements = $"Requirements: Password must be between 8 and 60 characters; contain at least 1 number, uppercase character, lowercase character; and a symbol ( {symbols} ).";

        if (password.Length < 8)
            return Result.Failure($"Password length does not meet requirements. {requirements}");

        if (password.Length > 60)
            return Result.Failure($"Password length does not meet requirements. {requirements}");

        if (!HasNumberRegex().IsMatch(password))
            return Result.Failure($"Password does not contain a number. {requirements}");

        if (!HasUpperCaseRegex().IsMatch(password))
            return Result.Failure($"Password does not contain an uppercase character. {requirements}");

        if (!HasLowerCaseRegex().IsMatch(password))
            return Result.Failure($"Password does not contain a lowercase character. {requirements}");

        if (!HasSymbolRegex().IsMatch(password))
            return Result.Failure($"Password does not contain a symbol. {requirements}");

        return Result.Success();
    }
}