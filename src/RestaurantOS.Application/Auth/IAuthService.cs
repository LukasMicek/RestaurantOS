using RestaurantOS.Application.Common;

namespace RestaurantOS.Application.Auth;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest req, CancellationToken ct);
    Task<Result<string>> LoginAsync(LoginRequest req, CancellationToken ct);
}