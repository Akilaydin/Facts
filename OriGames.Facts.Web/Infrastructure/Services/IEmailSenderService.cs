using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface IEmailSenderService
{
	Task<bool> SendAsync(EmailMessage message, CancellationToken token);
}