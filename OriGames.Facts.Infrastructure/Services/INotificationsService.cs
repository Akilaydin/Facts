namespace OriGames.Facts.Infrastructure.Services;

public interface INotificationsService
{
	Task SendScheduledNotificationsAsync(CancellationToken token);
}