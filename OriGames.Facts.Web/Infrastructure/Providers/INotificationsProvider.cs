namespace OriGames.Facts.Web.Infrastructure.Providers;

public interface INotificationsProvider
{
	Task ProcessNotificationAsync(CancellationToken token);
}