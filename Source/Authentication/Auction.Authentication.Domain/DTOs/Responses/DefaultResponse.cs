namespace Auction.Authentication.Domain.DTOs.Responses;

public class DefaultResponse(string? message)
{
	public string? Message { get; set; } = message;
}