using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface ISignUpUseCase
{
	Task<BaseActionResponse> ExecuteAsync(RegisterRequest request);
}