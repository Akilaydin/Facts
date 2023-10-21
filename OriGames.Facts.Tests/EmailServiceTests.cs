using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Services;

using Xunit;

namespace OriGames.Facts.Tests;

public class EmailServiceTests
{
	[Fact]
	public void AlwaysTrueEmailSendingService_Should_Return_True()
	{
		//Arrange
		var service = new MockAlwaysTrueEmailSenderService();

		//Act
		var result = service.SendAsync(new EmailMessage(), CancellationToken.None).Result;
		
		//Assert
		Assert.True(result);
	}
}