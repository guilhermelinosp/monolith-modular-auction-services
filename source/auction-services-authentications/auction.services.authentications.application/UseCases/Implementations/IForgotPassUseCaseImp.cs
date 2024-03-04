using auction.services.authentications.domain.DTOs.Abstracts;
using auction.services.authentications.domain.DTOs.Requests;

namespace auction.services.authentications.application.UseCases.Implementations;

public interface IForgotPassUseCaseImp
{
	Task<BaseActionResponse> ExecuteAsync(ForgotPassRequest request);
}