using Auction.Authentication.Domain.Notifications;
using Auction.Authentication.Domain.Repositories;
using Auction.Authentication.Infrastructure.Contexts;
using Auction.Authentication.Infrastructure.Contexts.Factories;
using Auction.Authentication.Infrastructure.Notifications;
using Auction.Authentication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Authentication.Infrastructure;

public static class InfrastructureInjection
{
	public static async Task AddInfrastructureInjection(this IServiceCollection services, IConfiguration configuration)
	{
		await AuthenticationDbContextFactory.CreateAsync(configuration["ConnectionStrings:MySql"]!);

		services.AddContexts(configuration);
		services.AddRepositories();
		services.AddNotifications();
	}

	private static void AddContexts(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AuthenticationDbContext>(options =>
			options.UseMySQL(configuration["ConnectionStrings:MySql"]!));
	}

	private static void AddRepositories(this IServiceCollection services)
	{
		services.AddScoped<IAccountRepository, AccountRepository>();
	}

	private static void AddNotifications(this IServiceCollection services)
	{
		services.AddSingleton<IProducerNotification, ProducerNotification>();
	}
}