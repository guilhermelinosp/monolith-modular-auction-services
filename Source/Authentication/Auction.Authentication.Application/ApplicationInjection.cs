using Auction.Authentication.Application.Services.AutoMapper;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.OneTimePass;
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
	public static async Task AddApplicationInjection(this IServiceCollection services, IConfiguration configuration)
	{
		await services.AddInfrastructureInjection(configuration);
		services.AddServices();
		services.AddUseCases();
	}

	private static void AddServices(this IServiceCollection services)
	{
		services.AddScoped<ICryptography, Cryptography>();
		services.AddScoped<ITokenization, Tokenization>();
		services.AddSingleton<IOneTimePass, OneTimePass>();

		services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
		services.AddScoped<AutoMapperProfiles>();
	}

	private static void AddUseCases(this IServiceCollection services)
	{
		services.AddScoped<ISignUpUseCase, SignUpUseCase>();
		services.AddScoped<ISignInUseCase, SignInUseCase>();
		services.AddScoped<IForgotPassUseCaseImp, ForgotPassUseCase>();
		services.AddScoped<IResetPassUseCase, ResetPassUseCase>();

		services.AddScoped<SignInValidator>();
		services.AddScoped<SignUpValidator>();
		services.AddScoped<ForgotPassValidator>();
		services.AddScoped<ResetPassValidator>();
	}
}