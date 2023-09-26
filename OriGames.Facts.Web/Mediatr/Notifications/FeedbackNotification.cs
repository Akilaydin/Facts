namespace OriGames.Facts.Web.Mediatr.Notifications;

public class FeedbackNotification : NotificationBase
{
	public FeedbackNotification(string content, Exception? exception = null) : 
		base("FEEDBACK", content, "artem@mail.ru", "NoReplyFacts@gmail.com", exception) { }
}