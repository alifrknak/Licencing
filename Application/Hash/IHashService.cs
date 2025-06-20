using Domain;
using Isopoh.Cryptography.Argon2;
using System.Security.Cryptography;
using System.Text;

namespace Application.Hash;

public interface IHashService
{
    PasswordModel GeneratePassword(string input);
    void Verify(string input, PasswordModel passwordModel);

}

public class Argon2HashService : IHashService
{
    private readonly Argon2Config _argon2Config;

    public Argon2HashService()
    {
        _argon2Config = new Argon2Config
        {
            Version = Argon2Version.Nineteen,
            TimeCost = 4,
            MemoryCost = 16,// 16 MB
            Lanes = 2,
            HashLength = 64,
        };
    }

    /// <summary>
    ///  Hash the input string using Argon2 algorithm.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>hashed version of the input</returns>
    public PasswordModel GeneratePassword(string input)
    {
        _argon2Config.Password = Encoding.UTF8.GetBytes(input);
        _argon2Config.Salt = RandomNumberGenerator.GetBytes(16);
        var hash = Argon2.Hash(_argon2Config);

        return new PasswordModel
        {
            PasswordHash = hash,
            Salt = _argon2Config.Salt
        };
    }

    /// <summary>
    /// Verify password throw exception when password is incorrect
    /// </summary>
    /// <param name="input"></param>
    /// <param name="passwordModel"></param>
    /// <exception cref="Exception"></exception>
    public void Verify(string input, PasswordModel passwordModel)
    {
        _argon2Config.Password = Encoding.UTF8.GetBytes(input);
        _argon2Config.Salt = passwordModel.Salt;
        var hash = Argon2.Hash(_argon2Config);
        if (hash != passwordModel.PasswordHash)
            throw new Exception();
    }
}
