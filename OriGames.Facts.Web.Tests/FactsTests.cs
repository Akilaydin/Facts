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
	[Trait("FactsService", "GetTwentyFacts_TwentyFacts")]
	public void GetTwentyFacts_Should_Return_20_Facts()
	{
		//Arrange
		var facts = _factService.GetLastTwentyFacts();
		
		//Act

		//Assert
		Assert.Equal(TestConstants.TwentyFactsCount, facts.Count());
	}
	
	[Fact]
	[Trait("FactsService", "GetTwentyFacts_NotNull")]
	public void GetTwentyFacts_Should_Not_Return_Null()
	{
		//Arrange
		var facts = _factService.GetLastTwentyFacts();
		
		//Act
		
		//Assert
		Assert.NotNull(facts);
	}
}
