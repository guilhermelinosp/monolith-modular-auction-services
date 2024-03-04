using System.Net;
using auction.services.authentications.application.Services.Cryptography;
using auction.services.authentications.application.Services.OneTimePass;
using auction.services.authentications.application.UseCases.Implementations;
using auction.services.authentications.application.UseCases.Validators;
using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;
using auction.services.authentications.domain.DTOs.Responses;
using auction.services.authentications.domain.Messages;
using auction.services.authentications.domain.Repositories;
using Serilog;

namespace auction.services.authentications.application.UseCases;

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