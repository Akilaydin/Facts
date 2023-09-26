using MediatR;

namespace OriGames.Facts.Web.Mediatr.Notifications;

public abstract class NotificationBase : INotification
{
	public string Subject { get;}
	public string Content { get; }
	public string From { get;  }
	public string To { get; }
	public Exception? Exception { get; }

	protected NotificationBase(string subject, string content, string from, string to, Exception? exception = null)
	{
		Subject = subject;
		Content = content;
		From = from;
		To = to;
		Exception = exception;
	}
}