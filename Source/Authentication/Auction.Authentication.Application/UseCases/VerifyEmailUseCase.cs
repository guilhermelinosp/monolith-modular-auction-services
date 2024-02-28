using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Authentication.Application.Services.Cryptography;
using Auction.Authentication.Application.Services.Tokenization;
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

public class VerifyEmailUseCase(
    IAccountRepository repository,
    VerifyEmailValidator validator,
    ITokenization tokenization,
    IProducerNotification producerNotification)
    : IVerifyEmailUseCaseImp
{

  public async Task<BaseActionResponse<DefaultResponse>> ExecuteAsync(VerifyEmailRequest request)
    {
        try
        {
            var requestValidation = await validator.ValidateAsync(request);
            if (!requestValidation.IsValid)
                return new BaseActionResponse<DefaultResponse>(false, null,
                    requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

            var account = await repository.FindByEmailAsync(request.Email);
            if (account == null)
                return new BaseActionResponse<DefaultResponse>(false, null,
                    new List<string> { ResponseMessage.ACCOUNT_NOT_FOUND });

            var code = tokenization.GenerateOneTimeToken();

            producerNotification.SendMessageAsync(new NotificaitonModel
            {
                Email = account.Email,
                Subject = "Confirmation code",
                Body = $"Your confirmation code is: {code}"
            });

            return new BaseActionResponse<DefaultResponse>(true, new DefaultResponse(ResponseMessage.SEND_CONFIRMATION_CODE), null);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while executing ConfirmEmailUseCase");
            throw;
        }
    }
}