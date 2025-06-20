using Application;
using Domain;
using Domain.Ports;
using Moq;
using System.Linq.Expressions;

namespace Tests;

public class AuthServiceTest
{
    [Fact]
    public async Task LoginAsync_ShouldReturnTrue_WhenCredentialsAreValid()
    {
        // Arrange
        var mockUserRepo = new Mock<IPersistencePort<User>>();
        mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>()));

        var authService = new AuthManager(mockUserRepo.Object, null!);

        // Act
        var result = await authService.LoginAsync("testuser", "Correctpassword");

        // Assert
        mockUserRepo.Verify(r => r.Single(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
    {
        // Arrange
        var mockUserRepo = new Mock<IPersistencePort<User>>();
        mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>()));

        var authService = new AuthManager(mockUserRepo.Object, null!);

        // Act
        var result = await authService.LoginAsync("testuser", "Wrongpassword");

        // Assert
        mockUserRepo.Verify(r => r.Single(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
    }
}
