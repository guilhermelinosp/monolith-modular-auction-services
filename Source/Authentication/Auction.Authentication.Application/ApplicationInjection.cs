using Auction.Authentication.Application.Services.AutoMapper;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.Tokenization;
using Auction.Authentication.Application.UseCases;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Authentication.Application;
public static class ApplicationInjection
{
	public static async Task AddApplicationInjection(this IServiceCollection services, IConfiguration configuration)	{
		await services.AddInfrastructureInjection(configuration);
		services.AddServices();
		services.AddUseCases();
	}
	
	private static void AddServices(this IServiceCollection services)
	{
		services.AddScoped<ICryptography, Cryptography>();
		services.AddScoped<ITokenization, Tokenization>();

		services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
		services.AddScoped<AutoMapperProfiles>();
	}
	
	private static void AddUseCases(this IServiceCollection services)
	{
		services.AddScoped<IRegisterUseCaseImp, RegisterUseCase>();
		services.AddScoped<ILoginUseCaseImp, LoginUseCase>();
		services.AddScoped<IVerifyEmailUseCaseImp, VerifyEmailUseCase>();
		services.AddScoped<IConfirmEmailUseCaseImp, ConfirmEmailUseCase>();
		
		services.AddScoped<LoginValidator>();
		services.AddScoped<RegisterValidator>();
		services.AddScoped<ConfirmEmailValidator>();
		services.AddScoped<VerifyEmailValidator>();
	}
}