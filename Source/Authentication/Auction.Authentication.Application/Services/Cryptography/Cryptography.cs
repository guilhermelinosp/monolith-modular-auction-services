using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace Auction.Authentication.Application.Services.Cryptography;

public class Cryptography : ICryptography
{
	public string EncryptPassword(string password)
	{
		try
		{
			var salt = GenerateSalt();
			var hash = GenerateHash(password, salt);
			return $"{salt}.{hash}";
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing EncryptPassword");
			throw;
		}
	}

	public bool VerifyPassword(string password, string hashedPassword)
	{
		try
		{
			var parts = hashedPassword.Split('.', 2);
			var salt = parts[0];
			var hash = parts[1];
			var hashedPasswordAttempt = GenerateHash(password, salt);
			return hash == hashedPasswordAttempt;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing VerifyPassword");
			throw;
		}
	}

	private static string GenerateSalt()
	{
		var salt = new byte[16];
		using var random = RandomNumberGenerator.Create();
		random.GetBytes(salt);
		return Convert.ToBase64String(salt);
	}

	private static string GenerateHash(string password, string salt)
	{
		var hashedInputBytes = SHA512.HashData(Encoding.UTF8.GetBytes(password + salt));
		return Convert.ToBase64String(hashedInputBytes);
	}
}