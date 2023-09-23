using System.Diagnostics;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

	public async Task<IActionResult> Index()
	{
		// if (await _roleManager.RoleExistsAsync(AppData.UserRole) == false)
		// {
		// 	await _roleManager.CreateAsync(new IdentityRole(AppData.UserRole));
		// 	await _roleManager.CreateAsync(new IdentityRole(AppData.AdministratorRole));
		// }
		//
		// const string userEmail = "test@t.com";
		// const string password = "test22";
		//
		// var user = new IdentityUser {
		// 	Email = userEmail,
		// 	EmailConfirmed = true,
		// 	NormalizedEmail = userEmail.ToUpper(),
		// 	PhoneNumber = "+79999622215",
		// 	UserName = userEmail,
		// 	PhoneNumberConfirmed = true,
		// 	NormalizedUserName = userEmail.ToUpper(),
		// 	SecurityStamp = Guid.NewGuid().ToString("D")
		// };
		// var user2 = await _userManager.FindByNameAsync(userEmail);
		// await _userManager.DeleteAsync(user2);
		// var result = await _userManager.CreateAsync(user, password);
		// if (result.Succeeded)
		// {
		// 	await _userManager.AddToRoleAsync(user, AppData.AdministratorRole);
		// }
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
