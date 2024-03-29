using System.Globalization;
using System.Net;
using auction.services.authentications.application.Services.Cryptography;
using auction.services.authentications.application.Services.OneTimePass;
using auction.services.authentications.application.Services.Tokenization;
using auction.services.authentications.application.UseCases.Implementations;
using auction.services.authentications.application.UseCases.Validators;
using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;
using auction.services.authentications.domain.DTOs.Responses;
using auction.services.authentications.domain.Messages;
using auction.services.authentications.domain.Models;
using auction.services.authentications.domain.Notifications;
using auction.services.authentications.domain.Repositories;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace auction.services.authentications.application.UseCases;

public class SignInUseCase(
	IAccountRepository repository,
	SignInValidator validator,
	IOneTimePass oneTimePass,
	IProducerNotification producerNotification,
	ITokenization tokenization,
	IConfiguration configuration,
	ICryptography cryptography)
	: ISignInUseCase
{
	public async Task<BaseActionResponse> ExecuteAsync(LoginRequest request)
	{
		try
		{
			var requestValidation = await validator.ValidateAsync(request);
			if (!requestValidation.IsValid)
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			var account = await repository.FindByEmailAsync(request.Email);
			if (account == null)
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			if (!cryptography.VerifyPassword(request.Password, account.Password))
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					new List<string> { DefaultMessage.PASSWORD_NOT_VALID });

			var code = oneTimePass.GenerateOtp(account.Id.ToString());

			var token = tokenization.GenerateToken(account.Id.ToString(), (int)account.Role);

			if (!account.Is2Fa)
				return new BaseActionResponse(
					HttpStatusCode.OK,
					new DefaultResponse(account.Id.ToString(), token),
					null);

			producerNotification.SendMessageAsync(
				new NotificaitonModel(
					account.Email,
					"otp sign in code",
					$"your otp code is: {code}"));

			return new BaseActionResponse(
				HttpStatusCode.Accepted,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.SEND_CONFIRMATION_CODE),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing SignInUseCase");
			throw;
		}
	}

	public async Task<BaseActionResponse<TokenResponse>> ConfirmSignInAsync(string accountId, string otp)
	{
		try
		{
			if (!oneTimePass.VerifyOtp(accountId, otp))
				return new BaseActionResponse<TokenResponse>(
					HttpStatusCode.BadRequest,
					null,
					new List<string> { DefaultMessage.OTP_NOT_VALID }
				);

			var account = await repository.FindByIdAsync(accountId);
			if (account == null)
				return new BaseActionResponse<TokenResponse>(
					HttpStatusCode.NotFound,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			var token = tokenization.GenerateToken(account.Id.ToString(), (int)account.Role);
			var refreshToken = tokenization.GenerateRefreshToken(account.Id.ToString());
			var expiryDate =
				DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:Expiry"]!, CultureInfo.InvariantCulture));

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "login confirmed",
				"Your login has been confirmed"));

			return new BaseActionResponse<TokenResponse>(
				HttpStatusCode.OK,
				new TokenResponse(token, refreshToken, expiryDate),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing ConfirmEmailAsync");
			throw;
		}
	}
}