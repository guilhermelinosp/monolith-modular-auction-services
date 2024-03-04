namespace auction.services.authentications.domain.DTOs.Responses;

public class ExceptionResponse(string message)
{
	public string Message { get; set; } = message;
}