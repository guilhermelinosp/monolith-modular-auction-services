namespace auction.services.authentications.domain.Messages;

public abstract class DefaultMessage
{
	public static string ACCOUNT_ALREADY_EXISTS = "account already exists";
	public static string ACCOUNT_CREATED = "account created successfully";
	public static string ACCOUNT_CONFIRMED = "account confirmed successfully";
	public static string ACCOUNT_NOT_FOUND = "account not found";
	public static string ACCOUNT_NOT_ACTIVATED = "account not activated";

	public static string PASSWORD_NOT_VALID = "password not valid";

	public static string FAILED_TO_SEND_CONFIRMATION_CODE = "failed to send confirmation code";
	public static string SEND_CONFIRMATION_CODE = "confirmation code sent your email";
	public static string OTP_NOT_VALID = "otp not valid or expired";
	public static string PASSWORD_CHANGED = "password changed successfully";
	public static string PASSWORD_NOT_CHANGED = "password not changed";
}