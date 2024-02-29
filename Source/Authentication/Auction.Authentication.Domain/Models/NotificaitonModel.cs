namespace Auction.Authentication.Domain.Models;

public class NotificaitonModel(string email, string subject, string body)
{
	public string Email { get; set; } = email;
	public string Subject { get; set; } = subject;
	public string Body { get; set; } = body;
}