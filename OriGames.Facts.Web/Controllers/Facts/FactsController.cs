using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Controllers.Facts.Queries;
using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers.Facts;

public class FactsController : Controller
{
	private readonly IMediator _mediator;

	public FactsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task<IActionResult> Index(int? pageIndex, string? tag, string? search)
	{
		ViewData["search"] = search;
		ViewData["tag"] = tag;
		
		var clampedPageIndex = pageIndex ?? 1;
		
		var request = new FactGetPagedRequest { PageIndex = clampedPageIndex, Tag = tag, Search = search };

		var response = await _mediator.Send(request, HttpContext.RequestAborted);

		if (response.Ok && response.Result.TotalPages < clampedPageIndex)
		{
			return RedirectToAction(nameof(Index), new {pageIndex = 1, tag, search});
		}

		return View(response);
	}
	
	public async Task<IActionResult> RandomFact()
	{
		var response = await _mediator.Send(new RandomFactRequest());
		
		return View(response);
	}
	
	[Authorize(Roles = AppData.AdministratorRole)]
	public async Task<IActionResult> Edit(Guid id, string returnUrl)
	{
		var operationResult = await _mediator.Send(new FactGetByIdForEditRequest(id, returnUrl));
		if (operationResult.Ok)
		{
			return View(operationResult.Result);
		}

		return RedirectToAction("Error", "Site", new { code = 404 });
	}

	[HttpPost]
	[Authorize(Roles = AppData.AdministratorRole)]
	public IActionResult Edit(FactEditViewModel model)
	{
		return !ModelState.IsValid 
			? View(model)
			: Redirect(model.ReturnUrl);

	}
	
	public IActionResult Cloud()
	{
		return View();
	}

	public async Task<IActionResult> RssChannel()
	{
		var response = await _mediator.Send(new FactRssRequest());
		
		return Content(response);
	}

	public async Task<IActionResult> Show(Guid factId, string? returnUrl = null)
	{
		var request = new FactGetByIdRequest { Id = factId };

		var fact = await _mediator.Send(request, HttpContext.RequestAborted);

		ViewBag.ReturnUrl = returnUrl!;
		
		return View(fact);
	}
}
