using Application.Hash;
using Domain;
using Domain.Dtos;
using Domain.Ports;
using System.ComponentModel.DataAnnotations;

namespace Application;

public class UserService(IPersistencePort<User> persistencePort, IHashService hashService, RequestContext requestContext, IUnitOfWork unitOfWork)
    : IUserService
{

    public async Task CreateUserAsync(CreateUserModel createUserModel)
    {
        ArgumentNullException.ThrowIfNull(createUserModel);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(createUserModel.FirstName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(createUserModel.LastName);

        //TODO: buradami olmalı yoksa  domain katmanında mı olmalı karar verilmeli
        var userExist = await persistencePort.ExistsAsync(x => x.Email == createUserModel.Email);
        if (userExist)
            throw new Exception("user's email already used");

        PasswordModel passwordModel = hashService.GeneratePassword(createUserModel.Password);

        User user = new User
        {
            FirstName = createUserModel.FirstName,
            LastName = createUserModel.LastName,
            Email = createUserModel.Email,
            PasswordModel = passwordModel,
            CreatedBy = requestContext.GetRequestOwnerId()
        };

        user.SetUserType(createUserModel.UserType, requestContext.GetRequestUserType());
        await persistencePort.AddAsync(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await persistencePort.DeleteAsync(x => x.Id == userId);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UpdateUserModel updateUserModel)
    {
        User user = new()
        {
            Email = updateUserModel.Email,
            FirstName = updateUserModel.FirstName,
            LastName = updateUserModel.LastName,
            LastModifiedOn = DateTime.UtcNow,
        };

        await persistencePort.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> RegisterAsync(RegisterUserDto registerUserDto)
    {
        await CreateUserAsync(new CreateUserModel
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email,
            Password = registerUserDto.Password,
            UserType = UserType.Member
        });
        return true;
    }
}


/// <summary>
/// it must be immutable.
/// </summary>
public record CreateUserModel
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }

    [MinLength(8)]
    public required string Password { get; init; }
    public required UserType UserType { get; init; }
}


public record UpdateUserModel
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }

    //[MinLength(8)]
    //public required string Password { get; init; }
    //public required UserType UserType { get; init; }
}