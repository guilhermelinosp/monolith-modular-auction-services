using auction.services.authentications.domain.Models;

namespace auction.services.authentications.domain.Notifications;

public interface IProducerNotification
{
	void SendMessageAsync(NotificaitonModel message);
}