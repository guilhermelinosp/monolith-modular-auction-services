using System.Net;
using auction.services.authentications.domain.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace auction.services.authentications.api.Filters;

public class ExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		Console.WriteLine(context.Exception);

		context.Result = new ObjectResult(
			new ExceptionResponse(
				"An error occurred while processing your request. Please try again later."))
		{
			StatusCode = (int)HttpStatusCode.InternalServerError
		};
		context.ExceptionHandled = true;
	}
}