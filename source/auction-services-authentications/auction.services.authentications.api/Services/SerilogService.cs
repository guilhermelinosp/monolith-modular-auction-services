using Serilog;
using Serilog.Events;

namespace auction.services.authentications.api.Services;

public static class SerilogService
{
	public static void AddSerilogServ(this IServiceCollection services)
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.WriteTo
			.File($"log-.txt", rollingInterval: RollingInterval.Day) // Aqui definimos o rollingInterval para 'Day'
			.CreateLogger();

		services.AddLogging(loggingBuilder =>
		{
			loggingBuilder.ClearProviders(); // Limpa os provedores de log padrão
			loggingBuilder.AddSerilog(); // Adiciona Serilog
		});
	}
}