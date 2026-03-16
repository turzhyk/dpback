
using DPBack.Application.Abstractions;
using DPBack.Application.Contracts;

using DPBack.Application.Validators;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace DPBack.Application.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _repo;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenProvider _tokenProvider;

    public UserService(IUsersRepository repo, IPasswordHasher<User> passwordHasher, ITokenProvider tokenProvider)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    public async Task<Guid> CreateUser(UserCreateRequest request)
    {
        if (!EmailValidator.IsValid(request.Email))
            throw new Exception("Invalid email");

        // if (await _repo.Пуе(request.Email))
        //     throw new Exception("User exists");

         var user = new User(
            Guid.NewGuid(),
            request.Email,
            "",
            request.Email,
            UserRole.User,
            DateTime.UtcNow
        );

        var hash = _passwordHasher.HashPassword(user, request.Password);

        user.SetPassword(hash); 

        await _repo.CreateAsync(user);

        return user.Id;
    }

    public async Task<string> Login(UserLoginRequest request)
    {
        var user = await GetByEmail(request.Login);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid login or password");
        
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if(result != PasswordVerificationResult.Success)
            throw new UnauthorizedAccessException("Invalid login or password");
        
        var token = _tokenProvider.Create(user);
        return token;
    }
    public async Task<User> GetByEmail(string email) => await _repo.GetByEmail(email);

    public async Task<List<UserAddressGetDto>> GetAdressesByUserId(Guid id)
    {
        var entities = await _repo.GetAdressesByUserId(id);
        
        return entities.Select(a => new UserAddressGetDto
        (
            a.Id,
            a.Country,
             a.City,
            a.Street,
            a.BuildingNumber,
            a.ApartmentNumber,
            a.PostalCode,
            a.PhoneNumber,
            a.Email,
            a.Options
        )).ToList();
    }

    public async Task AddUserAdress(Guid userId, UserAdressCreateDto dto)
    {
        var userAdress = new UserAdress(Guid.NewGuid(), userId, dto.Country, dto.City, dto.Street,
            dto.BuildingNumber, dto.ApartmentNumber, dto.PostalCode, dto.PhoneNumber, dto.Email,
            dto.Options);
        await _repo.AddUserAdress(userAdress);
    }

}