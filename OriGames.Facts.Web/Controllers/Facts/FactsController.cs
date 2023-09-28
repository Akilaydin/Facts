using MediatR;

using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Controllers.Facts.Queries;

namespace OriGames.Facts.Web.Controllers.Facts;

public class FactsController : Controller
{
	private readonly IMediator _mediator;

	public FactsController(IMediator mediator) {
		_mediator = mediator;
	}

	public async Task<IActionResult> Index(int? pageIndex, string? tag, string? search)
	{
		var request = new FactGetPagedRequest { PageIndex = pageIndex ?? 1, Tag = tag, Search = search };

		var response = await _mediator.Send(request, HttpContext.RequestAborted);
		
		return View(response);
	}
}
