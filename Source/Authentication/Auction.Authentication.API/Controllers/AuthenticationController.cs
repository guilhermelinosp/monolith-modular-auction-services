using System.Net;
using Auction.Authentication.API.Controllers.Abstracts;
using Auction.Authentication.Application.UseCases.Implementations;
using Auction.Authentication.Domain.DTOs.Requests;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Authentication.API.Controllers;

[ApiController]
[Route("api/auth/v{version:apiVersion}")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class AuthenticationController(
	ILoginUseCaseImp login,
	IVerifyEmailUseCaseImp verify,
	IRegisterUseCaseImp register) : Controller
{
	[HttpPost("login")]
	[ProducesResponseType<BaseActionResult<TokenResponse>>(StatusCodes.Status200OK)]
	public async Task<BaseActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
	{
		var response = await login.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult<TokenResponse>(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult<TokenResponse>(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("register")]
	[ProducesResponseType<BaseActionResult<DefaultResponse>>(StatusCodes.Status200OK)]
	public async Task<BaseActionResult<DefaultResponse>> Register([FromBody] RegisterRequest request)
	{
		var response = await register.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult<DefaultResponse>(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult<DefaultResponse>(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("refresh")]
	public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
	{
		return Ok();
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
	{
		return Ok();
	}

	[HttpPost("forgot-password")]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
	{
		return Ok();
	}

	[HttpPost("reset-password")]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
	{
		return Ok();
	}

	[HttpPost("verify-email")]
	[ProducesResponseType<BaseActionResult<DefaultResponse>>(StatusCodes.Status200OK)]
	public async Task<BaseActionResult<DefaultResponse>> VerifyEmail([FromBody] VerifyEmailRequest request)
	{
		var response = await verify.ExecuteAsync(request);

		return !response.Success
			? new BaseActionResult<DefaultResponse>(HttpStatusCode.BadRequest, null, response.Error)
			: new BaseActionResult<DefaultResponse>(HttpStatusCode.OK, response.Data, null);
	}

	[HttpPost("confirm-email")]
	public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
	{
		return Ok();
	}
}