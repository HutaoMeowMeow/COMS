using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto);
            return Ok(new { Message = "User registered successfully", User = user });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var token = await _userService.LoginAsync(dto);
        if (token == null)
            return Unauthorized(new { Message = "Invalid email or password." });

        var user = await _userService.GetByIdAsync(
            _jwtService.ValidateToken(token) ?? Guid.Empty);

        return Ok(new { Token = token, User = user });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var user = await _userService.GetByIdAsync(Guid.Parse(userId));
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
