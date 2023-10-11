using AutoBogus;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Data.FakeData;

namespace OriGames.Facts.Infrastructure.Data;

public class DbDataSeeder : IHostedService
{
	private const int FakeFactsCount = 500;
	private const int FakeTagsCount = 400;
	
	private const string SystemEmail = "System@test.com";
	private const string AdminEmail = "admin@gmail.com";
	private const string DefaultUserEmail = "user@gmail.com";
	
	private const string DefaultUserPassword = "user";
	private const string AdminPassword = "admin";

	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly ApplicationDbContext _dbContext;

	private readonly IServiceScope _scope;

	public DbDataSeeder(IServiceProvider serviceProvider)
	{
		_scope = serviceProvider.CreateScope();

		_roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		_userManager = _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
		_dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		await _dbContext.Database.EnsureCreatedAsync(cancellationToken);

		if (_dbContext.Facts.Any() == false)
		{
			SeedFactsAndTags(_dbContext);
		}

		if (await _userManager.FindByEmailAsync(AdminEmail) == null)
		{
			await SeedUsersAsync();
		}
		
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken _)
	{
		_scope.Dispose();
		
		return Task.CompletedTask;
	}

	private void SeedFactsAndTags(ApplicationDbContext context)
	{
		var factKeys = new Queue<Guid>(AutoFaker.Generate<Guid>(FakeFactsCount));
		var tagKeys = new Queue<Guid>(AutoFaker.Generate<Guid>(FakeTagsCount));

		var fakeFacts = new List<Fact>(FakeFactsCount);
		var fakeTags = new List<Tag>(FakeTagsCount);

		var verbWordsFaker = new AutoFaker<StringContainer>().RuleFor(c => c.Value, f => f.Hacker.Verb());
		var nounWordsFaker = new AutoFaker<StringContainer>().RuleFor(c => c.Value, f => f.Hacker.Noun());
		
		CreateFakeFacts();
		CreateFakeTags();

		foreach (var fakeFact in fakeFacts)
		{
			var takenTags = fakeTags.OrderBy(_ => Guid.NewGuid()).Take(Random.Shared.Next(1, 6)).ToList();
			
			fakeFact.Tags = takenTags.ToList();
		}

		var tagsWithFacts = fakeTags.Where(f => f.Facts.Count != 0);

		context.Facts.AddRange(fakeFacts);
		context.Tags.AddRange(tagsWithFacts);
		
		void CreateFakeTags()
		{
			for (int i = 0; i < FakeTagsCount; i++)
			{
				var fakeTag = AutoFaker.Generate<Tag, TagFaker>(builder => builder
					.WithArgs(
						tagKeys.Dequeue(),
						Random.Shared.NextSingle() > 0.5f ? verbWordsFaker : nounWordsFaker
					));

				//note: add only unique tags
				if (fakeTags.FirstOrDefault(tag => tag.Name == fakeTag.Name) == null)
				{
					fakeTags.Add(fakeTag);
				}
			}
		}
		
		void CreateFakeFacts()
		{
			for (int i = 0; i < FakeFactsCount; i++)
			{
				var fakeFact = AutoFaker.Generate<Fact, FactFaker>(builder => 
					builder.WithArgs(
						factKeys.Dequeue(),
						Random.Shared.NextSingle() > 0.5f ? verbWordsFaker : nounWordsFaker,
						SystemEmail
						));
				
				fakeFacts.Add(fakeFact);
			}
		}
	}
	
	private async Task SeedUsersAsync()
	{
		await CreateUserAsync(AdminEmail, AdminPassword, new IdentityRole(AppData.AdministratorRole));
		await CreateUserAsync(DefaultUserEmail, DefaultUserPassword, new IdentityRole(AppData.UserRole));

		async Task CreateUserAsync(string email, string password, IdentityRole role)
		{
			var newUser = new IdentityUser { Email = email, EmailConfirmed = true, NormalizedEmail = email.ToUpper(), NormalizedUserName = email.ToUpper(), UserName = email.ToLower() };
			
			await _userManager.CreateAsync(newUser, password);
			
			await _roleManager.CreateAsync(role);
			
			await _userManager.AddToRoleAsync(newUser, role.Name!);
		}
	}
}