using Application;
using Application.Hash;
using Domain;
using Domain.Dtos;
using Domain.Ports;
using Moq;

namespace Tests;

public class UserserviceTest
{
    [Fact]
    public async Task RegisterAsync_ShouldReturnTrue_WhenRegistrationSucceeds()
    {
        // Arrange
        var mockUserRepo = new Mock<IPersistencePort<User>>();
        mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>()));

        var userService = new UserService(mockUserRepo.Object, new Argon2HashService(), null!, null!);

        // Act
        var result = await userService.RegisterAsync(new RegisterUserDto
        {
            Email = "ValidEmail123@gmail.com",
            FirstName = "Test",
            LastName = "User",
            Password = "password123"
        });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFalse_WhenRegistrationFails()
    {
        // Arrange
        var mockUserRepo = new Mock<IPersistencePort<User>>();
        mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>()));

        var userService = new UserService(mockUserRepo.Object, new Argon2HashService(), null!, null!);

        // Act
        var result = await userService.RegisterAsync(new RegisterUserDto
        {
            Email = "INVALID",
            FirstName = "Test",
            LastName = "User",
            Password = "password123"
        });

        // Assert
        Assert.False(result);
    }
}
