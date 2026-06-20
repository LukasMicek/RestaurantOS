using RestaurantOS.Application.Common;

namespace RestaurantOS.Application.Auth;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterRequest req, CancellationToken ct);
}