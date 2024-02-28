using System;
using System.Net;
using Auction.Authentication.Domain.DTOs.Abstracts;
using Auction.Authentication.Domain.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auction.Authentication.API.Filters
{
	public class ExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			Console.WriteLine(context.Exception);
			
			context.Result = new ObjectResult(new DefaultResponse(
				"An error occurred while processing your request. Please try again later." 
			))
			{
				StatusCode = (int)HttpStatusCode.InternalServerError
			};
			context.ExceptionHandled = true;
		}
	}
}