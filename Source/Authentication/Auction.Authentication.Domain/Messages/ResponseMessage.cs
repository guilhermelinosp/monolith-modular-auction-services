namespace Auction.Authentication.Domain.Messages;

public abstract class ResponseMessage
{
	public static string ACCOUNT_ALREADY_EXISTS = "Account already exists";
	public static string ACCOUNT_CREATED = "Account created successfully";
	public static string ACCOUNT_NOT_FOUND = "Account not found";
	public static string PASSWORD_NOT_VALID = "Password not valid";
	public static string FAILED_TO_SEND_CONFIRMATION_CODE = "Failed to send confirmation code";
	public static string? SEND_CONFIRMATION_CODE = "Confirmation code sent successfully";

	public static string ERROR_UNKNOWN => "Unknown error.";

}