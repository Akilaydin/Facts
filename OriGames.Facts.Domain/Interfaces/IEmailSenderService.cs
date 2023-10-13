using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Domain.Interfaces;

public interface IEmailSenderService
{
	Task<bool> SendAsync(EmailMessage message, CancellationToken token);
}