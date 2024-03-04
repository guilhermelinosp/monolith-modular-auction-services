using System.Net;
using auction.services.authentications.application.Services.OneTimePass;
using auction.services.authentications.application.UseCases.Implementations;
using auction.services.authentications.application.UseCases.Validators;
using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;
using auction.services.authentications.domain.DTOs.Responses;
using auction.services.authentications.domain.Messages;
using auction.services.authentications.domain.Models;
using auction.services.authentications.domain.Notifications;
using auction.services.authentications.domain.Repositories;
using Serilog;

namespace auction.services.authentications.application.UseCases;

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
					HttpStatusCode.BadRequest,
					null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			var account = await repository.FindByEmailAsync(request.Email);
			if (account == null)
				return new BaseActionResponse(
					HttpStatusCode.NotFound,
					null,
					new List<string> { DefaultMessage.ACCOUNT_NOT_FOUND });

			var code = oneTimePass.GenerateOtp(account.Id.ToString());

			producerNotification.SendMessageAsync(new NotificaitonModel(account.Email, "forgot password code",
				$"your otp code is: {code}"));


			return new BaseActionResponse(
				HttpStatusCode.Accepted,
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