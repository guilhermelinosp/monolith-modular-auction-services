namespace Auction.Authentication.API.Services;

public static class CorsService
{
	public static void AddCorsServ(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(builder =>
			{
				builder
					.AllowAnyHeader()
					.AllowAnyMethod()
					.WithOrigins("*"); // Altere para a origem correta do seu cliente
			});
		});
	}
}