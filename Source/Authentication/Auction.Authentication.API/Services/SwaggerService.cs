namespace Auction.Authentication.API.Services;

public static class SwaggerService
{
	public static void AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
		});
	}
}