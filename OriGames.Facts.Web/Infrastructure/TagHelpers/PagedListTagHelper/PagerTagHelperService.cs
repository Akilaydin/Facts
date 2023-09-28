using OriGames.Facts.Web.Infrastructure.TagHelpers.PagedListTagHelper.Base;

namespace OriGames.Facts.Web.Infrastructure.TagHelpers.PagedListTagHelper;

public class PagerTagHelperService : IPagerTagHelperService
{
	public PagerContext GetPagerContext(int pageIndex, int pageSize, int totalPages, int pagesInGroup)
	{
		return null;
	}

	public IEnumerable<PagerPageBase> GetPages(PagerContext pagerContext)
	{
		yield break;
	}
}
