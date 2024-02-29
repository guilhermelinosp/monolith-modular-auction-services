using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.OneTimePass;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Models;
using Auction.Authentication.Domain.Notifications;
using Auction.Authentication.Domain.Repositories;
using Serilog;

namespace Auction.Authentication.Application.UseCases;

public class SignInUseCase(
	IAccountRepository repository,
	SignInValidator validator,
	IOneTimePass oneTimePass,
	IProducerNotification producerNotification,
	ICryptography cryptography) : ISignInUseCase
{
	public async Task<BaseActionResponse> ExecuteAsync(LoginRequest request)
	{
		try
		{
			var requestValidation = await validator.ValidateAsync(request);
			if (!requestValidation.IsValid)
				return new BaseActionResponse(
					false,
					null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			var account = await repository.FindByEmailAsync(request.Email);
			if (account == null)
				return new BaseActionResponse(
					false,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			if (!cryptography.VerifyPassword(request.Password, account.Password))
				return new BaseActionResponse(
					false,
					null,
					new List<string> { DefaultMessage.PASSWORD_NOT_VALID });

			var code = oneTimePass.GenerateOtp(account.Id.ToString());

			if (!account.IsActivated)
			{
				producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "otp active account code",
					$"your otp code is: {code}"));

				return new BaseActionResponse(
					false,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_ACTIVATED });
			}

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "otp sign in code",
				$"your otp code is: {code}"));

			return new BaseActionResponse(
				true,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.SEND_CONFIRMATION_CODE!),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing SignInUseCase");
			throw;
		}
	}
}