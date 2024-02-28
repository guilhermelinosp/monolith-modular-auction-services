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
using Auction.Authentication.Domain.Notifications;
using Auction.Authentication.Domain.Repositories;
using Serilog;

namespace Auction.Authentication.Application.UseCases;

public class ConfirmEmailUseCase(
    IAccountRepository repository,
    ConfirmEmailValidator validator,
    ITokenization tokenization,
    ICryptography cryptography,
    IProducerNotification producerNotification)
    : IConfirmEmailUseCaseImp
{

  public async Task<BaseActionResponse<DefaultResponse>> ExecuteAsync(ConfirmEmailRequest request)
    {
        try
        {
            var requestValidation = await validator.ValidateAsync(request);
            if (!requestValidation.IsValid)
                return new BaseActionResponse<DefaultResponse>(false, null,
                    requestValidation.Errors.Select(er => er.ErrorMessage).ToList());
            
            return new BaseActionResponse<DefaultResponse>(true, new DefaultResponse(""), null);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while executing ConfirmEmailUseCase");
            throw;
        }
    }
}