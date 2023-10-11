using System.Linq.Expressions;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Administrator.Queries;

public static class NotificationSelectors
{
	public static Expression<Func<Notification, NotificationViewModel>> Default => s => new NotificationViewModel
	{
		From = s.From,
		To = s.To,
		Content = s.Content,
		CreatedAt = s.CreatedAt,
		CreatedBy = s.CreatedBy,
		Id = s.Id,
		IsSent = s.IsSent,
		Title = s.Subject,
		UpdatedAt = s.UpdatedAt,
		UpdatedBy = s.UpdatedBy
	};
}
