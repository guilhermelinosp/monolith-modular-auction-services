namespace Auction.Authentication.Domain.DTOs.Responses;

public class TokenResponse(string token, string refreshToken, DateTime expires)
{
	public string? Token { get; set; } = token;
	public string? RefreshToken { get; set; } = refreshToken;
	public DateTime? Expires { get; set; } = expires;
}