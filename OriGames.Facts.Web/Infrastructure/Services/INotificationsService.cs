namespace OriGames.Facts.Web.Infrastructure.Services;

public interface INotificationsService
{
	Task SendScheduledNotificationsAsync(CancellationToken token);
}