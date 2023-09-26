using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

public class SiteController : Controller
{
	public IActionResult Index(int? pageIndex, string? tag, string? search)
	{
		ViewBag.Index = pageIndex;
		ViewBag.Tag = tag;
		ViewBag.Search = search;
		
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
