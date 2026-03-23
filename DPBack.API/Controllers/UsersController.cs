
using System.Security.Claims;
using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPBack.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("No active user found");
        }
        return Guid.Parse(userId);
    }
    public UsersController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser(UserCreateRequest request, CancellationToken cToken)
    {
        var id = await _service.CreateUser(request, cToken);
        return Ok(id);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] UserLoginRequest request, CancellationToken cToken)
    {
        try
        {
            var result = await _service.Login(request, cToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserByLogin(Guid id, CancellationToken cToken)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("adresses")]
    public async Task<ActionResult<List<UserAddressResponseDto>>> GetUserAddresses(CancellationToken cToken)
    {
        var userId = GetCurrentUserId();
        var result = await  _service.GetAddressesByUserId(userId, cToken);
        return Ok(result);
    }
    [Authorize]
    [HttpPost("addresses")]
    public async Task<ActionResult> AddUserAdress ([FromBody] UserAdressCreateDto request, CancellationToken cToken)
    {
        var userId = GetCurrentUserId();

        await _service.AddUserAddress(userId, request, cToken);
        return Ok();
    }
    [HttpPatch("addresses/{addressId}")]
    [Authorize]
    public async Task<ActionResult> ChangeUserAddress(Guid addressId, [FromBody] UserAdressCreateDto request, CancellationToken cToken)
    {
        // Implement later
        return Ok();
    }
    [HttpDelete("addresses/{addressId}")]
    [Authorize]
    public async Task<ActionResult> DeleteUserAddress(Guid addressId, CancellationToken cToken)
    {
        // Implement later
        return Ok();
    }
}