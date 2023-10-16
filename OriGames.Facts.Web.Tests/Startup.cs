using System.Configuration;

using Calabonga.UnitOfWork;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OriGames.Facts.Infrastructure.Data;
using OriGames.Facts.Infrastructure.Services;
using OriGames.Facts.Infrastructure.Services.HostedServices;
using OriGames.Facts.Web.TagHelpers.PagedListTagHelper;

namespace OriGames.Facts.Web.Tests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequireDigit = false;
			options.Password.RequireUppercase = false;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequiredLength = 0;
		});

		services.AddRouting(config =>
		{
			config.LowercaseQueryStrings = true;
			config.LowercaseUrls = true;
		});
		
		services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(TestConstants.ConnectionString));
		services.AddDatabaseDeveloperPageExceptionFilter();
	
		services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>();
	
		services.AddAutoMapper(typeof(Program).Assembly);
		services.AddMediatR(typeof(Program).Assembly);
		services.AddUnitOfWork<ApplicationDbContext>();

		services.AddControllersWithViews();
	
		// Dependencies
		services.AddTransient<IPagerTagHelperService, PagerTagHelperService>();
		services.AddTransient<IFactService, FactService>();
		services.AddTransient<IVersionInfoService, VersionInfoService>();
		services.AddTransient<ITagService, TagService>();
		services.AddTransient<ITagSearchService, TagSearchService>();
		services.AddTransient<INotificationsService, NotificationsService>();
		services.AddTransient<IEmailSenderService, MockAlwaysTrueEmailSenderService>();
	
		// Hosted services
		services.AddHostedService<NotificationsHostedService>();
		services.AddHostedService<DbDataSeeder>();

		services.AddResponseCaching();

		services.AddServerSideBlazor();
	}
}
