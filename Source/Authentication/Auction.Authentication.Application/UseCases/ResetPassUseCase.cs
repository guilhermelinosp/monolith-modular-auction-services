using System.Net;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.OneTimePass;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Models;
using Auction.Authentication.Domain.Repositories;
using Serilog;

namespace Auction.Authentication.Application.UseCases;

public class ResetPassUseCase(
	IAccountRepository repository,
	ResetPassValidator validator,
	IOneTimePass oneTimePass,
	ICryptography cryptography) : IResetPassUseCase
{
	public async Task<BaseActionResponse> ExecuteAsync(ResetPassRequest request, string accountId, string otp)
	{
		try
		{
			if (!oneTimePass.VerifyOtp(accountId, otp))
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					new List<string> { DefaultMessage.OTP_NOT_VALID }
				);

			var requestValidation = await validator.ValidateAsync(request);
			if (!requestValidation.IsValid)
				return new BaseActionResponse(
					HttpStatusCode.BadRequest,
					null,
					requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

			var account = await repository.FindByIdAsync(accountId);

			account!.Password = cryptography.EncryptPassword(request.Password);
			account.UpdatedAt = DateTime.UtcNow;

			await repository.UpdateAsync(account);

			return new BaseActionResponse(
				HttpStatusCode.OK,
				new DefaultResponse(account.Id.ToString(), DefaultMessage.PASSWORD_CHANGED!),
				null);
		}
		catch (Exception e)
		{
			Log.Error(e, "An error occurred while executing ResetPassUseCase");
			throw;
		}
	}
}