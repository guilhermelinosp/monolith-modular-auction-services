using Auction.Authentication.Domain.Models;

namespace Auction.Authentication.Domain.Notifications;

public interface IProducerNotification
{
	void SendMessageAsync(NotificaitonModel message);
}