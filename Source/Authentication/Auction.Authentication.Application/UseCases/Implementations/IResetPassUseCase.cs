using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface IResetPassUseCase
{
	Task<BaseActionResponse> ExecuteAsync(ResetPassRequest request, string accountId, string otp);
}