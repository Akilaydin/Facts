using System.Text;

using AutoBogus;

using Bogus;

using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Web.Data.FakeData;

public sealed class FactFaker : AutoFaker<Fact>
{
	public FactFaker(Guid id, Faker<StringContainer> contentFaker, string systemEmail)
	{
		RuleFor(fakeFact => fakeFact.Id, () => id);
		RuleFor(fakeFact => fakeFact.Content, () => GenerateFakeContent(contentFaker));
		RuleFor(fakeFact => fakeFact.CreatedBy, () => systemEmail);
		RuleFor(fakeFact => fakeFact.UpdatedBy, () => systemEmail);
		RuleFor(fakeFact => fakeFact.CreatedAt, faker => faker.Date.Past().ToUniversalTime());
		RuleFor(fakeFact => fakeFact.UpdatedAt, faker => faker.Date.Recent().ToUniversalTime());
		
		Ignore(fakeFact => fakeFact.Tags);
	}

	private string GenerateFakeContent(Faker<StringContainer> contentFaker)
	{
		var result = new StringBuilder();
			
		for (int i = 0; i < Random.Shared.Next(5, 15); i++)
		{
			result.Append(contentFaker.Generate().Value);
			result.Append(' ');
		}

		return result.ToString();
	}
}