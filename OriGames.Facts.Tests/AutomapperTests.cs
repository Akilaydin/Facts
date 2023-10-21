using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Mappers.Configurations;
using OriGames.Facts.Infrastructure.Services;

using Xunit;

namespace OriGames.Facts.Tests;

public class AutomapperTests
{
	[Fact]
	[Trait("Automapper", "Mapper Configuration")]
	public void Mapping_Should_Be_Correct()
	{
		//Arrange
		var config = MapperRegistration.GetMapperConfiguration();
		
		//Act

		//Assert
		config.AssertConfigurationIsValid();
	}

	
}
