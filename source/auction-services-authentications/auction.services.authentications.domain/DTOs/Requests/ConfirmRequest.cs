namespace auction.services.authentications.domain.DTOs.Requests;

public class ConfirmRequest
{
	public required string Email { get; set; }
	public required string Otp { get; set; }
}