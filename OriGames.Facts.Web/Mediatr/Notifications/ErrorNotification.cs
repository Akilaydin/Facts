namespace OriGames.Facts.Web.Mediatr.Notifications;

public class ErrorNotification : NotificationBase
{
	public ErrorNotification(string content, Exception? exception = null) : 
		base("ERROR", content, "artem@mail.ru", "NoReplyFacts@gmail.com", exception) { }
}