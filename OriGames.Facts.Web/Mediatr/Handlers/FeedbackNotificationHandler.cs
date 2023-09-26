using Calabonga.UnitOfWork;

using OriGames.Facts.Web.Mediatr.Notifications;

namespace OriGames.Facts.Web.Mediatr.Handlers;

public class FeedbackNotificationHandler : NotificationHandlerBase<FeedbackNotification>
{
	public FeedbackNotificationHandler(ILogger<FeedbackNotification> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork) { }
}