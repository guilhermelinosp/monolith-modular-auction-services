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

public class ForgotPassUseCase(
	IAccountRepository repository,
	ForgotPassValidator validator,
	IProducerNotification producerNotification,
	IOneTimePass oneTimePass) : IForgotPassUseCaseImp
{
	public async Task<BaseActionResponse> ExecuteAsync(ForgotPassRequest request)
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

			var code = oneTimePass.GenerateOtp(account.Id.ToString());

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "forgot password code",
				$"your otp code is: {code}"));


			return new BaseActionResponse(
				true,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.SEND_CONFIRMATION_CODE!),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing ForgotPassUseCase");
			throw;
		}
	}
}