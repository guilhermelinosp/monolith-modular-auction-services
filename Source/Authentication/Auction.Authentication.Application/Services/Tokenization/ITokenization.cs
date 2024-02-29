namespace Auction.Authentication.Application.Services.Tokenization;

public interface ITokenization
{
	string GenerateToken(string id);
	bool ValidateToken(string token);
	string GenerateRefreshToken(string key);
	bool VerifyRefreshToken(string key, string token);
}