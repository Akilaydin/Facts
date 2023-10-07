using AutoMapper;

using Calabonga.UnitOfWork;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Services;

namespace OriGames.Facts.Web.Infrastructure.Providers;

public class NotificationsProvider : INotificationsProvider
{
	private readonly IEmailSenderService _emailSenderService;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public NotificationsProvider(IEmailSenderService emailSenderService, IUnitOfWork unitOfWork, IMapper mapper)
	{
		_emailSenderService = emailSenderService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task ProcessNotificationAsync(CancellationToken token)
	{
		var notificationsRepository = _unitOfWork.GetRepository<Notification>();

		var notifications = notificationsRepository.GetAll(predicate: n => n.IsSent == false, n => n.OrderBy(notification => notification.CreatedAt)).ToList();

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
