
using System.Security.Claims;

using DPBack.Application.Contracts;
using DPBack.Application.Abstractions;

using DPBack.Domain.Models;
using DPBack.Infrastructure.TockenProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public async Task<ActionResult<Guid>> CreateUser(UserCreateRequest request)
    {
        var id = await _service.CreateUser(request);
        return Ok(id);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] UserLoginRequest request)
    {
        try
        {
            var result = await _service.Login(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }

    [Authorize]
    [HttpGet("adresses")]
    public async Task<ActionResult<List<UserAddressGetDto>>> GetUserAddresses()
    {
        var userId = GetCurrentUserId();
        var result = await  _service.GetAdressesByUserId(userId);
        return Ok(result);
    }
    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult> AddUserAdress ([FromBody] UserAdressCreateDto request)
    {
        var userId = GetCurrentUserId();

        await _service.AddUserAdress(userId, request);
        return Ok();
    }
    [HttpPatch("address/{addressId}")]
    [Authorize]
    public async Task<ActionResult> ChangeUserAddress(Guid addressId, [FromBody] UserAdressCreateDto request)
    {
        // Implement later
        return Ok();
    }
    [HttpDelete("address/{addressId}")]
    [Authorize]
    public async Task<ActionResult> DeleteUserAddress(Guid addressId)
    {
        // Implement later
        return Ok();
    }
}