using System.Globalization;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.Tokenization;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Auction.Authentication.Application.UseCases;

public class LoginUseCase(
	IConfiguration configuration,
	IAccountRepository repository,
	LoginValidator validator,
	IMapper mapper,
	ICryptography cryptography,
	ITokenization tokenization) : ILoginUseCaseImp
{
	public async Task<BaseActionResponse<TokenResponse>> ExecuteAsync(LoginRequest request)
	{
		try
		{
			var requestValidation = await validator.ValidateAsync(request);
			if (!requestValidation.IsValid)
				return new BaseActionResponse<TokenResponse>(false, null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			var account = await repository.FindByEmailAsync(request.Email);
			if (account == null)
				return new BaseActionResponse<TokenResponse>(false, null,
					new List<string> { ResponseMessage.ACCOUNT_NOT_FOUND });

			var hashedPassword = cryptography.VerifyPassword(request.Password, account.Password);
			if (!hashedPassword)
				return new BaseActionResponse<TokenResponse>(false, null,
					new List<string> { ResponseMessage.PASSWORD_NOT_VALID });

			var token = tokenization.GenerateToken(account.Id.ToString());

			var refreshToken = tokenization.GenerateRefreshToken();

			var expiryDate =
				DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:Expiry"]!, CultureInfo.InvariantCulture));

			return new BaseActionResponse<TokenResponse>(true, new TokenResponse(token, refreshToken, expiryDate), null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing LoginUseCase");
			throw;
		}
	}
}