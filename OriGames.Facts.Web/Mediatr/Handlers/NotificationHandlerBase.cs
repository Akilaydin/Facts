using System.Text;

using Calabonga.UnitOfWork;

using MediatR;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Extensions;
using OriGames.Facts.Web.Extensions;
using OriGames.Facts.Web.Mediatr.Notifications;

namespace OriGames.Facts.Web.Mediatr.Handlers;

public abstract class NotificationHandlerBase<T> : INotificationHandler<T> where T : NotificationBase
{
	private readonly ILogger<T> _logger;
	private readonly IUnitOfWork _unitOfWork;

	protected NotificationHandlerBase(ILogger<T> logger, IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_unitOfWork = unitOfWork;
	}

	async Task INotificationHandler<T>.Handle(T handledNotification, CancellationToken cancellationToken)
	{
		var notificationsRepository = _unitOfWork.GetRepository<Notification>();
		var notificationContentBuilder = new StringBuilder();

		notificationContentBuilder.AppendLine(handledNotification.Content);

		if (handledNotification.Exception is not null)
		{
			notificationContentBuilder.AppendLine(handledNotification.Exception.Message);
		}
		
		var newNotification = new Notification(
			handledNotification.Subject, 
			notificationContentBuilder.ToString(), 
			handledNotification.From, 
			handledNotification.To);

		await notificationsRepository.InsertAsync(newNotification, cancellationToken);

		await _unitOfWork.SaveChangesAsync();

		if (_unitOfWork.LastSaveChangesResult.IsOk == false)
		{
			_logger.LogDatabaseSavingError(nameof(Notification), _unitOfWork.LastSaveChangesResult.Exception);
			return;
		}

		_logger.LogNotificationAdded(newNotification.Subject);
	}
}