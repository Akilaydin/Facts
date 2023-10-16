using OriGames.Facts.Web.TagHelpers.PagedListTagHelper.Base;

namespace OriGames.Facts.Web.TagHelpers.PagedListTagHelper;

public class PagerPageDisabled : PagerPageBase
{
	public PagerPageDisabled(string title, int value) : base(title, value, false, true)
	{
	}
}
