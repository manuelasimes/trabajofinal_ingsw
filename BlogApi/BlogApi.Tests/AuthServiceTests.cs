using Xunit;
using FluentAssertions;
using BlogApi.AuthServices;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService();
    }

    [Fact]
    public void ValidateUser_ValidCredentials_ReturnsTrue()
    {
        // Act
        var result = _authService.ValidateUser("admin", "password");

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("admin", "wrongpassword")]
    [InlineData("nonexistent", "password")]
    public void ValidateUser_InvalidCredentials_ReturnsFalse(string username, string password)
    {
        // Act
        var result = _authService.ValidateUser(username, password);

        // Assert
        result.Should().BeFalse();
    }
}
