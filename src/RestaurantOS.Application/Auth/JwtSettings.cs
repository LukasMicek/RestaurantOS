namespace RestaurantOS.Application.Auth;

public class JwtSettings
{
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public string Secret { get; set; } = "";        // z user-secrets, NIE z repo
    public int AccessTokenMinutes { get; set; } = 60;
}