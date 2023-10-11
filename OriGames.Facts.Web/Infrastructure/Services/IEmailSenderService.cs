﻿using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface IEmailSenderService
{
	Task<bool> SendAsync(EmailMessage message, CancellationToken token);
}