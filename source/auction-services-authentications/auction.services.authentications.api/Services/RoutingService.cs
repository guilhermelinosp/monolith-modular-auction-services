namespace auction.services.authentications.api.Services;

public static class RoutingService
{
	public static void AddRoutingServ(this IServiceCollection services)
	{
		services.AddRouting(options =>
		{
			options.LowercaseUrls = true;
			options.LowercaseQueryStrings = true;
		});
	}
}