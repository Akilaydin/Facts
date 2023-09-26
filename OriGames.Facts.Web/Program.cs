using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Mappers.Base;

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
	
	MapperRegistration.GetMapperConfiguration();

	builder.Services.AddAutoMapper(typeof(Program).Assembly);
	
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

	app.MapControllerRoute(name: "index", pattern: "{controller=Site}/{action=Index}/{tag:regex([a-zА-Я])}/{search:regex([a-zА-Я])}/{pageIndex:int?}");
	app.MapControllerRoute(name: "index", pattern: "{controller=Site}/{action=Index}/{tag:regex([a-zА-Я])}/{pageIndex:int?}");
	app.MapControllerRoute(name: "index", pattern: "{controller=Site}/{action=Index}/{pageIndex:int?}");
	app.MapControllerRoute(name: "default", pattern: "{controller=Site}/{action=Index}/{id?}");
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