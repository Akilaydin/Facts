using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public class MockAlwaysTrueEmailSenderService : IEmailSenderService
{
	public async Task<bool> SendAsync(EmailMessage message, CancellationToken token)
	{
		await Task.Delay(TimeSpan.FromSeconds(5), token);
		
		return true;
	}
}
