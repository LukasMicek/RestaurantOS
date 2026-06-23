using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestaurantOS.Application.Auth;
using RestaurantOS.Application.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantOS.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth, IValidator<RegisterRequest> validator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            return ValidationProblem(new ValidationProblemDetails(
                validation.ToDictionary()));

        var result = await auth.RegisterAsync(req, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created)
            : BadRequest(new { errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
    {
        var result = await auth.LoginAsync(req, ct);
        return result.IsSuccess
            ? Ok(new { accessToken = result.Value })
            : Unauthorized(new { errors = result.Errors });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
        return Ok(new { id, email });
    }   
}