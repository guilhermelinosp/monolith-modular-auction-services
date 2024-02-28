namespace Auction.Authentication.Domain.Models;

public class NotificaitonModel
{
	public required string Email { get; set; }
	public required string Subject { get; set; }
	public required string Body { get; set; }
}