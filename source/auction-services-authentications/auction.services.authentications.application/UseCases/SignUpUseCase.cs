using System.Net;
using auction.services.authentications.application.Services.Cryptography;
using auction.services.authentications.application.Services.OneTimePass;
using auction.services.authentications.application.UseCases.Implementations;
using auction.services.authentications.application.UseCases.Validators;
using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Responses;
using auction.services.authentications.domain.Entities;
using auction.services.authentications.domain.Messages;
using auction.services.authentications.domain.Notifications;
using auction.services.authentications.domain.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Serilog;

namespace auction.services.authentications.application.UseCases;

public class SignUpUseCase(
	IAccountRepository repository,
	SignUpValidator validator,
	IOneTimePass oneTimePass,
	IProducerNotification producerNotification,
	ICryptography cryptography)
	: ISignUpUseCase
{
	public async Task<BaseActionResponse> ExecuteAsync(RegisterRequest request)
	{
		try
		{
			var requestValidation = await validator.ValidateAsync(request);
			if (!requestValidation.IsValid)
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			if (await repository.FindByEmailAsync(request.Email) != null)
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					new List<string> { DefaultMessage.ACCOUNT_ALREADY_EXISTS });

			var account = new Account
			{
				Email = request.Email,
				Password = cryptography.EncryptPassword(request.Password)
			};

			await repository.CreateAsync(account);

			return new BaseActionResponse(
				HttpStatusCode.Created,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.ACCOUNT_CREATED),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing SignUpUseCase");
			throw;
		}
	}
}