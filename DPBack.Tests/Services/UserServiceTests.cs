using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;
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
        var user = new User { Id = id };

        _mockRepository.Setup(r => r.GetById(id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _service.GetById(id, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetById_ShouldThrow_WhenUserNotFound()
    {
        var id = Guid.NewGuid();
        _mockRepository.Setup(x => x.GetById(id, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetById(id, CancellationToken.None));
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

    [Fact]
    public async Task CreateUser_ShouldReturnGuid()
    {
        var email = "user@mail.com";
        var password = "1111";
        var user = new User { Email = email };
        var passwordHash = "";

        _mockHasher.Setup(x => x.HashPassword(user, password)).Returns(passwordHash);
        var newUser = new User { Email = email, PasswordHash = passwordHash };
        var guid = Guid.NewGuid();
        _mockRepository.Setup(x => x.CreateAsync(
                It.Is<User>(u =>
                    u.Email == email &&
                    u.PasswordHash == passwordHash),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(guid);

        var result = await _service.CreateUser(new UserCreateRequest { Email = email, Password = password },
            CancellationToken.None);
        Assert.Equal(guid, result);
    }

    [Fact]
    public async Task GetUserAddresses_ShouldReturnMappedAddresses()
    {
        var id = Guid.NewGuid();
        List<UserAdress> addresses = new List<UserAdress>() { new UserAdress { Id = Guid.NewGuid() } };


        _mockRepository.Setup(x =>
            x.GetAdressesByUserId(id, It.IsAny<CancellationToken>())).ReturnsAsync(addresses);

        var result = await _service.GetAddressesByUserId(id, CancellationToken.None);
        Assert.Equal(result[0].Id, addresses[0].Id);
    }

    [Fact]
    public async Task GetUserAddresses_ShouldReturnEmptyList_WhenNoAddressesFound()
    {
        List<UserAdress> addresses = new List<UserAdress>();

        _mockRepository.Setup(x => x.GetAdressesByUserId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(addresses);
        var result = await _service.GetAddressesByUserId(Guid.NewGuid(), CancellationToken.None);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserAddresses_ShouldThrow_WhenNoUserFound()
    {
        _mockRepository.Setup(x => 
                x.UserWithIdExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetAddressesByUserId(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task AddUserAddress_ShouldReturnGuid()
    {
        _mockRepository.Setup(x => 
                x.UserWithIdExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockRepository.Setup(x => 
                x.AddUserAdress(It.IsAny<UserAdress>(), It.IsAny<CancellationToken>()));
        var dto = new UserAdressCreateDto("Poland", "Poznan",
            "Street", "1", "1", "60-000", "123123123",
            "test@mail.com", "");
        var result = await _service.AddUserAddress(Guid.NewGuid(), dto, CancellationToken.None);
        Assert.NotEqual(Guid.Empty, result);
    }
    [Fact]
    public async Task AddUserAddress_ShouldThrow_WhenNoUserFound()
    {
        _mockRepository.Setup(x => 
                x.UserWithIdExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.AddUserAddress(Guid.NewGuid(), It.IsAny<UserAdressCreateDto>(), CancellationToken.None));
    }
}