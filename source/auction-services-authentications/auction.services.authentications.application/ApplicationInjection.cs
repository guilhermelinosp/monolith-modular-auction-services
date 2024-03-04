using auction.services.authentications.application.Services.AutoMapper;
using auction.services.authentications.application.Services.Cryptography;
using auction.services.authentications.application.Services.OneTimePass;
using auction.services.authentications.application.Services.Tokenization;
using auction.services.authentications.application.UseCases;
using auction.services.authentications.application.UseCases.Implementations;
using auction.services.authentications.application.UseCases.Validators;
using auction.services.authentications.infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auction.services.authentications.application;

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