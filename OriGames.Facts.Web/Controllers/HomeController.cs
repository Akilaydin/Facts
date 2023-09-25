using System.Diagnostics;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager; 
	
	public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		_logger = logger;
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public IActionResult Index([FromServices] ApplicationDbContext dbContext)
	{
		using var transaction = dbContext.Database.BeginTransaction();
		
		var fact = new Fact {
			Content = "test"
		};

		dbContext.Facts.Add(fact);

		dbContext.SaveChanges();
		
		transaction.Rollback();
		
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
