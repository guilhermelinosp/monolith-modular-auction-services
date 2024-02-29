using System.Globalization;
using Auction.Authentication.Application.Services.OneTimePass;
using Auction.Authentication.Application.Services.Tokenization;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Models;
using Auction.Authentication.Domain.Notifications;
using Auction.Authentication.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Auction.Authentication.Application.UseCases;

public class ConfirmUseCase(
	IAccountRepository repository,
	ITokenization tokenization,
	IConfiguration configuration,
	IOneTimePass oneTimePass,
	IProducerNotification producerNotification)
	: IConfirmUseCaseImp
{
	public async Task<BaseActionResponse> ConfirmEmailAsync(string accountId, string otp)
	{
		try
		{
			if (!oneTimePass.VerifyOtp(accountId, otp))
				return new BaseActionResponse(
					false,
					null,
					new List<string> { DefaultMessage.OTP_NOT_VALID }
				);

			var account = await repository.FindByIdAsync(accountId);
			if (account == null)
				return new BaseActionResponse(
					false,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			account.IsActivated = true;

			await repository.UpdateAsync(account);

			return new BaseActionResponse(
				true,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.ACCOUNT_CONFIRMED),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing ConfirmEmailAsync");
			throw;
		}
	}

	public async Task<BaseActionResponse<TokenResponse>> ConfirmSignInAsync(string accountId, string otp)
	{
		try
		{
			if (!oneTimePass.VerifyOtp(accountId, otp))
				return new BaseActionResponse<TokenResponse>(
					false,
					null,
					new List<string> { DefaultMessage.OTP_NOT_VALID }
				);

			var account = await repository.FindByIdAsync(accountId);
			if (account == null)
				return new BaseActionResponse<TokenResponse>(
					false,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			account.IsActivated = true;

			await repository.UpdateAsync(account);

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "account activated",
				"Your account has been activated"));

			var token = tokenization.GenerateToken(account.Id.ToString());
			var refreshToken = tokenization.GenerateRefreshToken(account.Id.ToString());
			var expiryDate =
				DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:Expiry"]!, CultureInfo.InvariantCulture));

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "login confirmed",
				"Your login has been confirmed"));

			return new BaseActionResponse<TokenResponse>(
				true,
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