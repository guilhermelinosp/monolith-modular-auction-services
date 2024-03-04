namespace auction.services.authentications.domain.Models;

public class OneTimePassModel(string token, DateTime expiry)
{
	public string Token { get; set; } = token;
	public DateTime Expiry { get; set; } = expiry;
}