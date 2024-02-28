using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface IVerifyEmailUseCaseImp
{
	Task<BaseActionResponse<DefaultResponse>> ExecuteAsync(VerifyEmailRequest request);
}