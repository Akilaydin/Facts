using AutoBogus;

using Bogus;

using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Web.Data.FakeData;

public sealed class TagFaker : AutoFaker<Tag>
{
	public TagFaker(Guid id, Faker<StringContainer> contentFaker)
	{
		RuleFor(fakeTag => fakeTag.Id, () => id);
		RuleFor(fakeTag => fakeTag.Name, () => contentFaker.Generate().Value);
		
		Ignore(fakeTag => fakeTag.Facts);
	}
}