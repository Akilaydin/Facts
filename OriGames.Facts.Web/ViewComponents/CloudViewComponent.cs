using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Infrastructure.Services;

namespace OriGames.Facts.Web.ViewComponents;

public class CloudViewComponent : ViewComponent
{
	private readonly ITagService _tagService;

	public CloudViewComponent(ITagService tagService) {
		_tagService = tagService;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var tagCloud = await _tagService.GetTagCloudAsync();

		return View(tagCloud);
	}
}
