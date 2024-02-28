using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface IConfirmEmailUseCaseImp
{
	Task<BaseActionResponse<DefaultResponse>> ExecuteAsync(ConfirmEmailRequest request);
}