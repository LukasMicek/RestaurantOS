using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RestaurantOS.Application.Auth;
using RestaurantOS.Application.Common;
using RestaurantOS.Domain.Entities;

namespace RestaurantOS.Infrastructure.Auth;

public class AuthService(UserManager<ApplicationUser> users, ITokenService tokens, ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result> RegisterAsync(RegisterRequest req, CancellationToken ct)
    {
        var user = new ApplicationUser { UserName = req.Email, Email = req.Email, DisplayName = req.DisplayName };
        var result = await users.CreateAsync(user, req.Password);
        if (!result.Succeeded)
        {
            logger.LogWarning("Registration failed for {Email}: {Errors}",
                req.Email, string.Join("; ", result.Errors.Select(e => e.Description)));
            return Result.Failure(result.Errors.Select(e => e.Description).ToArray());
        }
        logger.LogInformation("User registered: {UserId}", user.Id);
        return Result.Success();
    }

    public async Task<Result<string>> LoginAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(req.Email);
        if (user is null || !await users.CheckPasswordAsync(user, req.Password))
        {
            logger.LogWarning("Failed login attempt for {Email}", req.Email);
            return Result<string>.Failure("Invalid credentials.");
        }
        return Result<string>.Success(tokens.CreateAccessToken(user));
    }
}