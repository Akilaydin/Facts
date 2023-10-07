using Calabonga.Microservices.BackgroundWorkers;

using OriGames.Facts.Web.Extensions;
using OriGames.Facts.Web.Infrastructure.Providers;

namespace OriGames.Facts.Web.Infrastructure.HostedServices;

public class NotificationsHostedService : ScheduledHostedServiceBase
{
	#if DEBUG
		protected override string Schedule => "*/1 * * * *"; //note: every 1 minute
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

		var notificationsProvider = scope.ServiceProvider.GetService<INotificationsProvider>();
		
		await notificationsProvider!.ProcessNotificationAsync(token);
		
		Logger.LogNotificationProcessed(DateTime.Now.ToString("F"));
	}
}