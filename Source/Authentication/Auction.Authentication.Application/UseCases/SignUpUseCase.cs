using System.Net;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Entities;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Repositories;
using Serilog;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.OneTimePass;
using Auction.Authentication.Domain.Models;
using Auction.Authentication.Domain.Notifications;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases;

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