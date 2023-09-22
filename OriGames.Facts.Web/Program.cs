using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console(theme: AnsiConsoleTheme.Code)
	.CreateLogger();

try
{
	var builder = WebApplication.CreateBuilder(args);

	builder.Services.Configure<IdentityOptions>(options =>
	{
		options.Password.RequireDigit = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
	});
	
	builder.Host.UseSerilog();
	
	var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
	builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
	builder.Services.AddDatabaseDeveloperPageExceptionFilter();

	builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
		.AddEntityFrameworkStores<ApplicationDbContext>();
	
	builder.Services.AddControllersWithViews();
	
	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseMigrationsEndPoint();
	}
	else
	{
		app.UseExceptionHandler("/Home/Error");
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthorization();

	app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
	app.MapRazorPages();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}

async Task CreateRoles(IServiceProvider serviceProvider)
{
	var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

	foreach (var roleName in AppData.Roles)
	{
		var roleExist = await roleManager.RoleExistsAsync(roleName);
		if (!roleExist)
		{
			await roleManager.CreateAsync(new IdentityRole(roleName));
		}
	}
	const string userEmail = "test@t.com";
	const string password = "test";
	var poweruser = new IdentityUser
	{
		UserName = userEmail,
		Email = userEmail,
	};
	string userPWD = password;
	var _user = await userManager.FindByEmailAsync(userEmail);

	if(_user == null)
	{
		var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
		if (createPowerUser.Succeeded)
		{
			await userManager.AddToRoleAsync(poweruser, "Admin");

		}
	}
}