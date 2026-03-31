using DPBack.Application.Abstractions;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DPBack.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUsersRepository> _mockRepository;
    private readonly Mock<ITokenProvider> _mockToken;
    private readonly Mock<IPasswordHasher<User>> _mockHasher;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly IUserService _service;

    public UserServiceTests()
    {
        _mockHasher = new Mock<IPasswordHasher<User>>();
        _mockToken = new Mock<ITokenProvider>();
        _mockRepository = new Mock<IUsersRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _service = new UserService(_mockRepository.Object, _mockHasher.Object, _mockToken.Object, _mockLogger.Object);
    }
    [Fact]
    public async Task GetById_ShouldReturnUser()
    {
        var id = Guid.NewGuid();
        var user = new User{Id= id};

        _mockRepository.Setup(r => r.GetById(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _service.GetById(id, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }
    [Fact]
    public async Task GetByEmail_ShouldReturnUser()
    {
        var email = "test@mail.com";
        var user = new User { Email = email };

        _mockRepository.Setup(r => r.GetByEmail(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _service.GetByEmail(email, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }
    [Fact]
    public async Task GetByEmail_ShouldThrow_WhenUserNotFound()
    {
        var email = "notfound@mail.com";

        _mockRepository
            .Setup(r => r.GetByEmail(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetByEmail(email, CancellationToken.None));
    }
}