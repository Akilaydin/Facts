using Calabonga.AspNetCore.Controllers.Extensions;
using Calabonga.UnitOfWork;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Mappers.Base;
using OriGames.Facts.Web.Infrastructure.TagHelpers.PagedListTagHelper;

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

	builder.Services.AddRouting(config =>
	{
		config.LowercaseQueryStrings = true;
		config.LowercaseUrls = true;
	});
	
	builder.Host.UseSerilog();
	
	var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext)) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
	builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
	builder.Services.AddDatabaseDeveloperPageExceptionFilter();

	
	builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
		.AddEntityFrameworkStores<ApplicationDbContext>();
	
	MapperRegistration.GetMapperConfiguration();

	builder.Services.AddAutoMapper(typeof(Program).Assembly);
	builder.Services.AddMediatR(typeof(Program).Assembly);
	builder.Services.AddUnitOfWork<ApplicationDbContext>();

	builder.Services.AddControllersWithViews();

	builder.Services.AddTransient<IPagerTagHelperService, PagerTagHelperService>();

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

	app.MapControllerRoute(name: "index", pattern: "{controller=Facts}/{action=Index}/{tag:regex([a-zА-Я])}/{search:regex([a-zА-Я])}/{pageIndex:int?}");
	app.MapControllerRoute(name: "index", pattern: "{controller=Facts}/{action=Index}/{tag:regex([a-zА-Я])}/{pageIndex:int?}");
	app.MapControllerRoute(name: "index", pattern: "{controller=Facts}/{action=Index}/{pageIndex:int?}");
	app.MapControllerRoute(name: "default", pattern: "{controller=Facts}/{action=Index}/{id?}");
	app.MapRazorPages();
	
	#region disable some pages
	app.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() =>
		context.Response.Redirect("/Identity/Account/Login?returnUrl=~%2F", true, true)));

	app.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => 
		context.Response.Redirect("/Identity/Account/Login?returnUrl=~%2F", true, true)));

	#endregion

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