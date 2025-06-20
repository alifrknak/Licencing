using Domain.Dtos;

namespace Application;

public interface IUserService
{
    Task CreateUserAsync(CreateUserModel createUserModel);

    Task UpdateUserAsync(UpdateUserModel updateUserModel);

    Task DeleteUserAsync(Guid userId);

    Task<bool> RegisterAsync(RegisterUserDto registerUserDto);

}


