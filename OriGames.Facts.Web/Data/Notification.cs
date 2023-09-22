using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Web.Data;

public class Notification : Auditable
{
	public string Subject { get; set; }
	public string Content { get; set; }
	public bool IsSent { get; set; }
	public string From { get; set; }
	public string To { get; set; }
}
