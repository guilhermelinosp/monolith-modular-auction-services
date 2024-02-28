using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Auction.Authentication.Application.Services.Tokenization;

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


	public string GenerateToken(string id)
	{
		try
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("id", id)
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

	public string GenerateRefreshToken()
	{
		try
		{
			var salt = new byte[32];
			using var random = RandomNumberGenerator.Create();
			random.GetBytes(salt);
			return Convert.ToBase64String(salt);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateRefreshToken");
			throw;
		}
	}
	
	public string GenerateOneTimeToken()
	{
		try
		{
			return new Random().Next(100000, 1000000).ToString("D6");
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing GenerateOneTimeToken");
			throw;
		}
	}
}