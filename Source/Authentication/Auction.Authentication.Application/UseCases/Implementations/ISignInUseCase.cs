using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface ISignInUseCase
{
	Task<BaseActionResponse> ExecuteAsync(LoginRequest request);
}