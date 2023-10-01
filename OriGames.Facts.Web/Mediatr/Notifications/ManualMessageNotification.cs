namespace OriGames.Facts.Web.Mediatr.Notifications;

public class ManualMessageNotification : NotificationBase
{

	public ManualMessageNotification(string subject, string content, string from, string to, Exception? exception = null) 
		: base(subject, content, from, to, exception) { }
}
