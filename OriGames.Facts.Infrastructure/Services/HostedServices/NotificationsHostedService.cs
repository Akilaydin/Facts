using Calabonga.Microservices.BackgroundWorkers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OriGames.Facts.Infrastructure.Extensions;

namespace OriGames.Facts.Infrastructure.Services.HostedServices;

public class NotificationsHostedService : ScheduledHostedServiceBase
{
	#if DEBUG
		protected override string Schedule => "*/5 * * * *"; //note: every 5 minuts
	#else
		protected override string Schedule => "*/1 * * * *"; //note: every 1 minute
	#endif
	protected override string DisplayName => "Notifications scheduler";

	protected override bool IsExecuteOnServerRestart => true;

	public NotificationsHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<NotificationsHostedService> logger) 
		: base(serviceScopeFactory, logger) { }

	protected override async Task ProcessInScopeAsync(IServiceProvider serviceProvider, CancellationToken token)
	{
		using var scope = serviceProvider.CreateScope();

		var notificationsProvider = scope.ServiceProvider.GetService<INotificationsService>();
		
		await notificationsProvider!.SendScheduledNotificationsAsync(token);
		
		Logger.LogNotificationProcessed(DateTime.Now.ToString("F"));
	}
}