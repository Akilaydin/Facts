using AutoBogus;

using OriGames.Facts.Web.Data.FakeData;

namespace OriGames.Facts.Web.Data;

public static class DbDataSeeder
{
	private const int FakeFactsCount = 500;
	private const int FakeTagsCount = 400;
	private const string SystemEmail = "System@test.com";
	
	public static WebApplication SeedData(this WebApplication app)
	{
		using var scope = app.Services.CreateScope();
		
		using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

		try
		{
			context.Database.EnsureCreated();

			if (context.Facts.Any())
			{
				return app;
			}

			Seed(context);

			context.SaveChanges();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		return app;
	}

	private static void Seed(ApplicationDbContext context)
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
}