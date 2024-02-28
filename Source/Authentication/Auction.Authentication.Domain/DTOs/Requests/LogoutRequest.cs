namespace Auction.Authentication.Domain.DTOs.Requests;

public class LogoutRequest
{
	public required string Token { get; set; }
}