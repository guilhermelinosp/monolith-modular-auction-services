using auction.services.authentications.domain.Notifications;
using auction.services.authentications.domain.Repositories;
using auction.services.authentications.infrastructure.Contexts;
using auction.services.authentications.infrastructure.Contexts.Factories;
using auction.services.authentications.infrastructure.Notifications;
using auction.services.authentications.infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auction.services.authentications.infrastructure;

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