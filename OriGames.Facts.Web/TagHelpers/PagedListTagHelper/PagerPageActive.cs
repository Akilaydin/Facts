using OriGames.Facts.Web.TagHelpers.PagedListTagHelper.Base;

namespace OriGames.Facts.Web.TagHelpers.PagedListTagHelper;

public class PagerPageActive : PagerPageBase
{
	public PagerPageActive(string title, int value) : base(title, value, true)
	{
	}
}
