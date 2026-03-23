using DPBack.Application.Abstractions;
using DPBack.Application.Services;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DPBack.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUsersRepository> _mockRepository;
    private readonly Mock<ITokenProvider> _mockToken;
    private readonly Mock<IPasswordHasher<User>> _mockHasher;
    private readonly IUserService _service;

    public UserServiceTests()
    {
        _mockHasher = new Mock<IPasswordHasher<User>>();
        _mockToken = new Mock<ITokenProvider>();
        _mockRepository = new Mock<IUsersRepository>();
        _service = new UserService(_mockRepository.Object, _mockHasher.Object, _mockToken.Object);
    }

    public async Task GetByEmail_ShouldReturnUser()
    {
        
    }
}