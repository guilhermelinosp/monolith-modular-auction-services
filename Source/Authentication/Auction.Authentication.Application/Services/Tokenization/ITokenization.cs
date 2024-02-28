namespace Auction.Authentication.Application.Services.Tokenization;

public interface ITokenization
{
	string GenerateToken(string id);
	string GenerateRefreshToken();
	string GenerateOneTimeToken();
	bool ValidateToken(string token);
}