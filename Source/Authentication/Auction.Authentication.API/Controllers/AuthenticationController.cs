using System.Net;
using Auction.Authentication.API.Controllers.Abstracts;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Auction.Authentication.Domain.DTOs.Requests.LoginRequest;

namespace Auction.Authentication.API.Controllers;

[ApiController]
[Route("api/auth/v{version:apiVersion}")]
[ApiVersion("1.0")]
[Produces("application/json")]
[ProducesResponseType<BaseActionResult>(StatusCodes.Status200OK)]
public class AuthenticationController(
	ISignInUseCase signIn,
	IConfirmUseCaseImp confirm,
	IForgotPassUseCaseImp forgot,
	IResetPassUseCase reset,
	ISignUpUseCase signUp) : Controller
{
	[HttpPost("signup")]
	public async Task<BaseActionResult> SignUp([FromBody] RegisterRequest request)
	{
		var response = await signUp.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("signout")]
	public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
	{
		return Ok();
	}

	[HttpPost("signin")]
	public async Task<BaseActionResult> SignIn([FromBody] LoginRequest request)
	{
		var response = await signIn.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("signin/{accountId}/{otp}")]
	[ProducesResponseType<BaseActionResult<TokenResponse>>(StatusCodes.Status200OK)]
	public async Task<BaseActionResult<TokenResponse>> ConfirmSignIn([FromRoute] string accountId,
		[FromRoute] string otp)
	{
		var response = await confirm.ConfirmSignInAsync(accountId, otp);

		return !response.Success
			? new BaseActionResult<TokenResponse>(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult<TokenResponse>(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("refresh")]
	public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
	{
		return Ok();
	}

	[HttpPost("forgot-password")]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassRequest request)
	{
		var response = await forgot.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("reset-password/{accountId}/{otp}")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPassRequest request, [FromRoute] string accountId,
		[FromRoute] string otp)
	{
		var response = await reset.ExecuteAsync(request, accountId, otp);

		return !response.Success
			? new BaseActionResult(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("confirm-email/{accountId}/{otp}")]
	public async Task<IActionResult> ConfirmEmail([FromRoute] string accountId, [FromRoute] string otp)
	{
		var response = await confirm.ConfirmEmailAsync(accountId, otp);

		return !response.Success
			? new BaseActionResult(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult(HttpStatusCode.OK, response.Data, null);
	}
}