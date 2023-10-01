using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Mediatr.Notifications;

public class FeedbackNotification : NotificationBase
{
	public FeedbackNotification(FeedbackViewModel feedbackViewModel) : 
		base("FEEDBACK", feedbackViewModel.ToString()!, "artem@mail.ru", "NoReplyFacts@gmail.com", null) { }
}