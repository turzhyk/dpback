
using System.Security.Claims;
using DPBack.API.TockenProvider;
using DPBack.Application.Contracts;
using DPBack.Application.Interfaces;
using DPBack.Application.Validators;
using DPBack.Domain.Enums;
using DPBack.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DPBack.Application.Validators;

namespace DPBack.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser(UserCreateRequest request)
    {
        var id = await _service.CreateUser(request);
        return Ok(id);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] UserLoginRequest request,
        [FromServices] IPasswordHasher<User> passwordHasher, TokenProvider tokenProvider)
    {
        var user = await _service.GetByEmail(request.Login);
        if(user == null)
            throw new Exception("user not found");
        
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if(result != PasswordVerificationResult.Success)
            return Unauthorized();
        var token = tokenProvider.Create(user);
        return Ok(token);
    }

    [Authorize]
    [HttpGet("addessesGet")]
    public async Task<ActionResult<List<UserAddressGetDto>>> GetUserAddresses()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no active user found");
        }
        var result = await  _service.GetAdressesByUserId(new Guid(userId));
        return Ok(result);
    }
    [Authorize]
    [HttpPost("adressAdd")]
    public async Task<ActionResult> AddUserAdress ([FromBody] UserAdressCreateDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new Exception("no active user found");
            // return NotFound();
        }

        await _service.AddUserAdress(userId, request);
        return Ok();
    }
}