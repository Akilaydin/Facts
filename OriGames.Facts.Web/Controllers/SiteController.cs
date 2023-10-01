using System.Diagnostics;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Mediatr.Notifications;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

public class SiteController : Controller
{
	public IActionResult About() => View();
        
	// Calabonga: WHAT I MADE 7
	public IActionResult RandomFact() => View();
        
	// Calabonga: WHAT I MADE 8
	public IActionResult Cloud() => View();
        
	// Calabonga: WHAT I MADE 9
	public IActionResult Feedback() => View();
        
	// Calabonga: WHAT I MADE 10
	public IActionResult Rss() => View();
	
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
