using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auction.Authentication.API.Services;

public static class AuthenticationService
{
	public static void AddAuthenticationServ(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication()
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true
				};

				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
			})
			.AddCookie(options =>
			{
				options.Cookie.Name = "RefreshToken";
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
			});
	}
}