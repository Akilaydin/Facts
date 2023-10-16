using AutoMapper;

using Calabonga.UnitOfWork;

using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Infrastructure.Services;

public class NotificationsService : INotificationsService
{
	private readonly IEmailSenderService _emailSenderService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public NotificationsService(IEmailSenderService emailSenderService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_emailSenderService = emailSenderService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task SendScheduledNotificationsAsync(CancellationToken token)
	{
		var notificationsRepository = _unitOfWork.GetRepository<Notification>();

		var notifications = Enumerable.ToList<Notification>(notificationsRepository.GetAll(predicate: n => n.IsSent == false, n => Queryable.OrderBy<Notification, DateTime>(n, notification => notification.CreatedAt)));

		if (notifications.Any() == false)
		{
			return;
		}

		var messagesToSend = _mapper.Map<IEnumerable<EmailMessage>>(notifications);

		foreach (var message in messagesToSend)
		{
			var sentSuccessfully = await _emailSenderService.SendAsync(message, token);

			if (sentSuccessfully)
			{
				NotificationSent(Guid.Parse(message.Id));
			}
		}
	}

	private void NotificationSent(Guid id)
	{
		var notificationsRepository = _unitOfWork.GetRepository<Notification>();

		var sentNotification = notificationsRepository.GetFirstOrDefault(predicate: n => n.Id == id, disableTracking: false);

		if (sentNotification == null)
		{
			return;
		}

		sentNotification.IsSent = true;
		
		notificationsRepository.Update(sentNotification);
		
		_unitOfWork.SaveChanges();
	}
}
