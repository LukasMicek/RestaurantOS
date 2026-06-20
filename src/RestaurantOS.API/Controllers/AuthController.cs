using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RestaurantOS.Application.Auth;
using RestaurantOS.Application.Common;

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
}