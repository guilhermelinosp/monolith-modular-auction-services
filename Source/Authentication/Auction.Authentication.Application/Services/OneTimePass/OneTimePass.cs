using System.Text.Json;
using Auction.Authentication.Domain.Models;
using Serilog;

namespace Auction.Authentication.Application.Services.OneTimePass;

public class OneTimePass : IOneTimePass
{
	private readonly Dictionary<string, string> _otpCache = new();

	public string GenerateOtp(string key)
	{
		try
		{
			var otp = new Random().Next(100000, 1000000).ToString("D6");

			_otpCache[key] =
				JsonSerializer.Serialize(new OneTimePassModel(otp, DateTime.UtcNow.Add(TimeSpan.FromMinutes(5))));

			return otp;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateOTP");
			throw;
		}
	}

	public bool VerifyOtp(string key, string otp)
	{
		try
		{
			if (!_otpCache.TryGetValue(key, out var json)) return false;

			var otpData = JsonSerializer.Deserialize<OneTimePassModel>(json);

			if (otpData == null || otpData.Token != otp || otpData.Expiry < DateTime.UtcNow) return false;

			_otpCache.Remove(key);
			return true;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing VerifyOtp");
			throw;
		}
	}
}