using Auction.Authentication.Domain.DTOs.Requests;

namespace Auction.Authentication.Domain.Messages;

public abstract class ValidatorMessage
{
	public static string EMAIL_NOT_INFORMED = "email not informed";
	public static string EMAIL_NOT_VALID = "email not valid";
	public static string PASSWORD_NOT_INFORMED = "password not informed";
	public static string PASSWORD_NOT_VALID = "password not valid";
	public static string PASSWORD_MINIMUM_LENGTH = "password must have at least 8 characters";
	public static string OTP_NOT_INFORMED = "otp not informed";
	public static string OTP_LENGTH = "otp must have 6 characters";
	public static string OTP_NOT_VALID = "otp not valid";
}