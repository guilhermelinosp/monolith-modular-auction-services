using Auction.Authentication.Domain.DTOs.Requests;

namespace Auction.Authentication.Domain.Messages;

public abstract class ValidatorMessage
{
	public static string EMAIL_NOT_INFORMED = "Email not informed";
	
	public static string EMAIL_NOT_VALID = "Email not valid";
	
	public static string PASSWORD_NOT_INFORMED = "Password not informed";
	
	public static string PASSWORD_NOT_VALID = "Password not valid";

	public static string PASSWORD_MINIMUM_LENGTH = "Password must have at least 8 characters";

	public static string OTP_NOT_INFORMED = "OTP not informed";
	
	public static string OTP_LENGTH = "OTP must have 6 characters";
	
	public static string OTP_NOT_VALID = "OTP not valid";
}