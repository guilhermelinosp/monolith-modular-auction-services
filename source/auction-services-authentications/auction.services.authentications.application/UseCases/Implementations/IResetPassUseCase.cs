using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;

namespace auction.services.authentications.application.UseCases.Implementations;

public interface IResetPassUseCase
{
	Task<BaseActionResponse> ExecuteAsync(ResetPassRequest request, string accountId, string otp);
}