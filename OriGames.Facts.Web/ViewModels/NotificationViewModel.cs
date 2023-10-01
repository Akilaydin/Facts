using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Web.ViewModels;

public class NotificationViewModel: Identity
{
	public string Title { get; set; } = null!;

	public string? Content { get; set; }

	public bool IsSent { get; set; }

	public DateTime CreatedAt { get; set; }

	public string CreatedBy { get; set; } = null!;

	public DateTime? UpdatedAt { get; set; }

	public string? UpdatedBy { get; set; }

	public string To { get; set; } = null!;

	public string From { get; set; } = null!;
}
