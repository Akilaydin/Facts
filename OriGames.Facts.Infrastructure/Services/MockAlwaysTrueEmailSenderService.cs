using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Infrastructure.Services;

public class MockAlwaysTrueEmailSenderService : IEmailSenderService
{
	public async Task<bool> SendAsync(EmailMessage message, CancellationToken token)
	{
		await Task.Delay(TimeSpan.FromSeconds(5), token);
		
		return true;
	}
}
