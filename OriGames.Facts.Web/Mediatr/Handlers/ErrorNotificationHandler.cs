using Calabonga.UnitOfWork;

using OriGames.Facts.Web.Mediatr.Notifications;

namespace OriGames.Facts.Web.Mediatr.Handlers;

public class ErrorNotificationHandler : NotificationHandlerBase<ErrorNotification>
{
	public ErrorNotificationHandler(ILogger<ErrorNotification> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork) { }
}