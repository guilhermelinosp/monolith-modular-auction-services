using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;

namespace Auction.Authentication.Application.UseCases.Implementations;

public interface IForgotPassUseCaseImp
{
	Task<BaseActionResponse> ExecuteAsync(ForgotPassRequest request);
}