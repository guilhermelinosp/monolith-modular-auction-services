using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using auction.services.authentications.domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace auction.services.authentications.application.Services.Tokenization;

public class Tokenization(IConfiguration configuration) : ITokenization
{
	public bool ValidateToken(string token)
	{
		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!)),
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true
			};

			tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;

			return jwtToken.ValidTo >= DateTime.UtcNow;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateToken");
			throw;
		}
	}


	public string GenerateToken(string id, int role)
	{
		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("id", id),
					new Claim("role", role.ToString("D1"))
				}),
				Expires = DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:Expiry"]!, CultureInfo.CurrentCulture)),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!)),
					SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateToken");
			throw;
		}
	}

	private readonly Dictionary<string, RefreshTokenModel> _otpCacheRefreshToken = new();

	public string GenerateRefreshToken(string key)
	{
		try
		{
			var salt = new byte[32];
			using var random = RandomNumberGenerator.Create();
			random.GetBytes(salt);
			var refreshToken = Convert.ToBase64String(salt);
			var expiry = DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:Expiry"]!, CultureInfo.CurrentCulture));

			_otpCacheRefreshToken[key] = new RefreshTokenModel(refreshToken, expiry);

			return refreshToken;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateRefreshToken");
			throw;
		}
	}

	public bool VerifyRefreshToken(string key, string token)
	{
		try
		{
			if (!_otpCacheRefreshToken.TryGetValue(key, out var model))
				return false;

			if (model.Token != token || model.Expiry < DateTimeOffset.UtcNow)
				return false;

			_otpCacheRefreshToken.Remove(key);
			return true;
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing VerifyRefreshToken");
			throw;
		}
	}
}