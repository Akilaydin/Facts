using Ardalis.GuardClauses;

using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Domain.Data;

public class Notification : Auditable
{
	public string Subject { get; set; }
	public string Content { get; set; }
	public string From { get; set; }
	public string To { get; set; }
	public bool IsSent { get; set; }

	public Notification(string subject, string content, string from, string to)
	{
		Subject = Guard.Against.NullOrWhiteSpace(subject, nameof(subject));
		Content = Guard.Against.NullOrWhiteSpace(content, nameof(content));
		From = Guard.Against.NullOrWhiteSpace(from, nameof(from));
		To = Guard.Against.NullOrWhiteSpace(to, nameof(to));
	}
}
