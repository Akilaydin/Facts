using OriGames.Facts.Infrastructure.Mappers.Configurations;

using Xunit;

namespace OriGames.Facts.Web.Tests;

public class AutomapperTests
{
	[Fact]
	[Trait("Automapper", "Mapper Configuration")]
	public void ItShouldBeCorrectlyConfigured()
	{
		//Arrange
		var config = MapperRegistration.GetMapperConfiguration();
		
		//Act

		//Assert
		config.AssertConfigurationIsValid();
	}
}
