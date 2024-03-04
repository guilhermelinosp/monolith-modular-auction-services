using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface ISignInUseCase
{
	Task<BaseActionResponse> ExecuteAsync(LoginRequest request);
	Task<BaseActionResponse<TokenResponse>> ConfirmSignInAsync(string accountId, string otp);
}