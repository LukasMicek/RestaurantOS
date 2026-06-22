using RestaurantOS.Domain.Entities;

namespace RestaurantOS.Application.Auth;

public interface ITokenService 
{ 
    string CreateAccessToken(ApplicationUser user); 
}