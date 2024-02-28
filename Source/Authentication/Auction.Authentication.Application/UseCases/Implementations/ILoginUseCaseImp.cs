using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface ILoginUseCaseImp
{
	Task<BaseActionResponse<TokenResponse>> ExecuteAsync(LoginRequest request);
}