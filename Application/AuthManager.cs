using Application.Hash;
using Domain;
using Domain.Ports;

namespace Application;

public class AuthManager(IPersistencePort<User> persistencePort, IHashService hashService) : IAuthService
{

    public async Task<string> LoginAsync(string email, string password)
    {
        User user = await persistencePort.Single(x => x.Email.Equals(email));
        hashService.Verify(password, user.PasswordModel);
        return "";
    }
}