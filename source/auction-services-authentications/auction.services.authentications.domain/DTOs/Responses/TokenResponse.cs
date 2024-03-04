namespace auction.services.authentications.domain.DTOs.Responses;

public class TokenResponse(string token, string refreshToken, DateTime expire)
{
	public string Token { get; set; } = token;
	public string RefreshToken { get; set; } = refreshToken;
	public DateTime Expire { get; set; } = expire;
}