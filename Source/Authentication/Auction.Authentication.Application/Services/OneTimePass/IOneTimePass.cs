namespace Auction.Authentication.Application.Services.OneTimePass;

public interface IOneTimePass
{
	public string GenerateOtp(string key);
	public bool VerifyOtp(string key, string otp);
}