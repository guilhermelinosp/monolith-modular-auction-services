namespace Auction.Authentication.Domain.DTOs.Responses;

public class ExceptionResponse(string message)
{
	public string Message { get; set; } = message;
}