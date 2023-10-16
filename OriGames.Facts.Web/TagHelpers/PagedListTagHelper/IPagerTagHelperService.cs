using OriGames.Facts.Web.TagHelpers.PagedListTagHelper.Base;

namespace OriGames.Facts.Web.TagHelpers.PagedListTagHelper;

public interface IPagerTagHelperService
{
	PagerContext GetPagerContext(int pageIndex, int pageSize, int totalPages, int pagesInGroup);

	IEnumerable<PagerPageBase> GetPages(PagerContext pagerContext);
}