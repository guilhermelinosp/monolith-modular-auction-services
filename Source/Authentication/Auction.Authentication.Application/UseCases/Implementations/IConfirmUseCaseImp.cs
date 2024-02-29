using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface IConfirmUseCaseImp
{
	Task<BaseActionResponse> ConfirmEmailAsync(string accountId, string otp);
	Task<BaseActionResponse<TokenResponse>> ConfirmSignInAsync(string accountId, string otp);
}