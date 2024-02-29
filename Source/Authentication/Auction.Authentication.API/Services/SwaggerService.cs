using Microsoft.OpenApi.Models;

namespace Auction.Authentication.API.Services;

public static class SwaggerService
{
	public static void AddSwaggerServ(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "Documentation", Version = "v1" });
		});
	}

	public static void UseSwaggerDoc(this IApplicationBuilder app)
	{
		app.UseSwagger(c =>
		{
			c.SerializeAsV2 = true;
			c.RouteTemplate = "swagger/{documentName}/swagger.json";
		});

		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentation");
			options.DocumentTitle = "Documentation";
		});
	}
}