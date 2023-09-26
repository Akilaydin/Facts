using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Web.Data;

public class Notification : Auditable
{
	public string Subject { get; set; }
	public string Content { get; set; }
	public bool IsSent { get; set; }
	public string From { get; set; }
	public string To { get; set; }
	
	public Notification(string subject, string content, string from, string to)
	{
		Subject = subject;
		Content = content;
		From = from;
		To = to;
	}
}
