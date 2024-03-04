using auction.services.authentications.domain.DTOs.Abstracts;
using Microsoft.AspNetCore.Identity.Data;

namespace auction.services.authentications.application.UseCases.Implementations;

public interface ISignUpUseCase
{
	Task<BaseActionResponse> ExecuteAsync(RegisterRequest request);
}