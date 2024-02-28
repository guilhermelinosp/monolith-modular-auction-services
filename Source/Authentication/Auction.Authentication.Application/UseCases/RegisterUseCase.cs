using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Application.UseCases.Validators;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Auction.Authentication.Domain.Entities;
using Auction.Authentication.Domain.Messages;
using Auction.Authentication.Domain.Repositories;
using Serilog;
using Auction.Authentication.Application.Services.Cryptography;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases
{
    public class RegisterUseCase(
        IAccountRepository repository,
        RegisterValidator validator,
        ICryptography cryptography)
        : IRegisterUseCaseImp
    {
        public async Task<BaseActionResponse<DefaultResponse>> ExecuteAsync(RegisterRequest request)
        {
            try
            {
                var requestValidation = await validator.ValidateAsync(request);
                if (!requestValidation.IsValid)
                    return new BaseActionResponse<DefaultResponse>(false, null, requestValidation.Errors.Select(er => er.ErrorMessage).ToList());

                if (await repository.FindByEmailAsync(request.Email) != null)
                    return new BaseActionResponse<DefaultResponse>(false, null, new List<string> { ResponseMessage.ACCOUNT_ALREADY_EXISTS });
                
                await repository.CreateAsync(new Account { Email = request.Email, Password = cryptography.EncryptPassword(request.Password) });

                return new BaseActionResponse<DefaultResponse>(true, new DefaultResponse(ResponseMessage.ACCOUNT_CREATED), null);
            }
            catch (Exception e)
            {
                Log.Error(e, "An error occurred while executing RegisterUseCase");
                throw;
            }
        }
    }
}
