using OriGames.Facts.Infrastructure.Services;

using Xunit;

namespace OriGames.Facts.Web.Tests;

public class FactsTests
{
	private readonly IFactService _factService;
	
	public FactsTests(IFactService factService)
	{
		_factService = factService;
	}
	
	[Fact]
	[Trait("FactsService", "GetTwentyFacts")]
	public void GetTwentyFacts_Should_Return_20_Facts()
	{
		//Arrange
		var facts = _factService.GetLastTwentyFacts();
		
		//Act

		//Assert
		Assert.Equal(TestConstants.TwentyFactsCount, facts.Count());
	}
}
