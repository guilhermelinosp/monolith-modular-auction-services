namespace Auction.Authentication.Domain.DTOs.Requests;

public class ConfirmEmailRequest
{
	public required string Email { get; set; }
	public required string Otp { get; set; }
}