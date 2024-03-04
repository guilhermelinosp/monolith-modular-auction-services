using System.Net;
using auction.services.authentications.domain.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace auction.services.authentications.api.Controllers.Abstracts;

public class BaseActionResult<T>(HttpStatusCode? status, T? data, List<string>? error) : IActionResult
{
	public int? Status => (int)status!;
	public T? Data => data;
	public List<string>? Error => error;


	public Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(new { status = Status, data = Data, error = Error })
		{
			StatusCode = (int)status!
		};

		return objectResult.ExecuteResultAsync(context);
	}
}

public class BaseActionResult(HttpStatusCode? status, DefaultResponse? data, List<string>? error) : IActionResult
{
	public int? Status => (int)status!;
	public DefaultResponse? Data => data;
	public List<string>? Error => error;

	public Task ExecuteResultAsync(ActionContext context)
	{
		var objectResult = new ObjectResult(new { status = Status, data = Data, error = Error })
		{
			StatusCode = (int)status!
		};

		return objectResult.ExecuteResultAsync(context);
	}
}