using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;
using auction.services.authentications.domain.DTOs.Responses;

namespace auction.services.authentications.application.UseCases.Implementations;

public interface ISignInUseCase
{
	Task<BaseActionResponse> ExecuteAsync(LoginRequest request);
	Task<BaseActionResponse<TokenResponse>> ConfirmSignInAsync(string accountId, string otp);
}