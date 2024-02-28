namespace Auction.Authentication.Domain.Exceptions;

[Serializable]
public class ValidatorException : SystemException
{
	public ValidatorException()
	{
	}

	public ValidatorException(List<string>? messages)
	{
		Messages = messages;
	}

	public List<string>? Messages { get; set; }
}