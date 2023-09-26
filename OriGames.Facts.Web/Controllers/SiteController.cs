using System.Diagnostics;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Mediatr.Notifications;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

public class SiteController : Controller
{
	private readonly IMediator _mediator;

	public SiteController(IMediator mediator) {
		_mediator = mediator;
	}

	public IActionResult Index(int? pageIndex, string? tag, string? search)
	{
		ViewBag.Index = pageIndex;
		ViewBag.Tag = tag;
		ViewBag.Search = search;
		
		return View();
	}

	public async Task<IActionResult> Privacy()
	{
		await _mediator.Publish(new ErrorNotification("Error notification from Privacy", new Exception("Exception")));
		await _mediator.Publish(new FeedbackNotification("Feedback notification from Privacy", new Exception("Exception")));
		
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
