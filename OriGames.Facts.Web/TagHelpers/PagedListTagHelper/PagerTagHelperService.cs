using OriGames.Facts.Web.TagHelpers.PagedListTagHelper.Base;

namespace OriGames.Facts.Web.TagHelpers.PagedListTagHelper;

public class PagerTagHelperService : IPagerTagHelperService
{
	private const string LabelPrevious = "«";
	private const string LabelNext = "»";
	private const string LabelLast = "»»";
	private const string LabelFirst = "««";

	PagerContext IPagerTagHelperService.GetPagerContext(int pageIndex, int pageSize, int totalCount, int pagesInGroup)
	{
		var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
		var groupIndex = (int)Math.Floor(Convert.ToDecimal(pageIndex) / Convert.ToDecimal(pagesInGroup));
		var minPage = groupIndex * pagesInGroup + 1;
		var maxPage = minPage + pagesInGroup - 1;
		var prevPage = minPage - 1;
		var nextPage = maxPage + 1;

		if (minPage <= 1)
		{
			prevPage = 1;
		}

		if (maxPage > totalPages)
		{
			maxPage = totalPages;
		}

		if (nextPage > totalPages)
		{
			nextPage = 0;
		}

		return new PagerContext {
			PageIndex = pageIndex,
			TotalPages = totalPages,
			GroupIndex = groupIndex,
			MinPage = minPage,
			MaxPage = maxPage,
			NextPage = nextPage,
			PreviousPage = prevPage
		};
	}

	IEnumerable<PagerPageBase> IPagerTagHelperService.GetPages(PagerContext pager)
	{
		var list = new List<PagerPageBase>();

		var firstAndPrev = GetPreviousPages(pager);
		list.AddRange(firstAndPrev);

		var pages = GetNumberPages(pager);
		list.AddRange(pages);

		var nextAndLast = GetNextPages(pager);
		list.AddRange(nextAndLast);

		return list;
	}

	private IEnumerable<PagerPageBase> GetPreviousPages(PagerContext pager)
	{
		yield return pager.PageIndex == 0 ? new PagerPageDisabled(LabelFirst, 1) : new PagerPage(LabelFirst, 1);

		yield return pager.PreviousPage > 1 ? new PagerPage(LabelPrevious, pager.MinPage - 1) : new PagerPageDisabled(LabelPrevious, pager.MinPage - 1);
	}

	private IEnumerable<PagerPageBase> GetNumberPages(PagerContext pager)
	{
		for (var i = pager.MinPage; i <= pager.MaxPage; i++)
		{
			if (i == pager.PageIndex + 1)
			{
				yield return new PagerPageActive(i.ToString(), i);
				continue;
			}
			yield return new PagerPage(i.ToString(), i);
		}
	}

	private IEnumerable<PagerPageBase> GetNextPages(PagerContext pager)
	{
		yield return pager.NextPage >= pager.MaxPage ? new PagerPage(LabelNext, pager.MaxPage + 1) : new PagerPageDisabled(LabelNext, pager.MaxPage);

		yield return pager.PageIndex + 1 == pager.TotalPages ? new PagerPageDisabled(LabelLast, pager.TotalPages) : new PagerPage(LabelLast, pager.TotalPages);
	}
}
