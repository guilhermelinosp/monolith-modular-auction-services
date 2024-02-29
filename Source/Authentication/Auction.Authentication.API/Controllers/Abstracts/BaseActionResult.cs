using System.Net;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Authentication.API.Controllers.Abstracts;

public class BaseActionResult<T>(HttpStatusCode statusCode, T? data, List<string>? error) : IActionResult
{
	public T? Data => data;
	public List<string>? Error => error;

	public Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(new { data = Data, error = Error })
		{
			StatusCode = (int)statusCode
		};

		return objectResult.ExecuteResultAsync(context);
	}
}

public class BaseActionResult(HttpStatusCode statusCode, DefaultResponse? data, List<string>? error) : IActionResult
{
	public DefaultResponse? Data => data;
	public List<string>? Error => error;

	public Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(new { data = Data, error = Error })
		{
			StatusCode = (int)statusCode
		};

		return objectResult.ExecuteResultAsync(context);
	}
}