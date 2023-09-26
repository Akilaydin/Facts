namespace OriGames.Facts.Web.Extensions;

static class EventIdentifiers
{
	public static readonly EventId DatabaseSavingErrorId = new(70040001, "DatabaseSavingError");
	public static readonly EventId NotificationSavedId = new(70040002, "NotificationSaved");
}

public static class LoggerExtensions
{
	public static void LogNotificationAdded(this ILogger logger, string subject, Exception? exception = null)
	{
		DatabaseSavingErrorExecute(logger, subject, exception);
	}

	private static readonly Action<ILogger, string, Exception?> NotificationAddedExecute =
		LoggerMessage.Define<string>(LogLevel.Information, EventIdentifiers.NotificationSavedId, "New notification created on subject: {subject}");
	
	public static void LogDatabaseSavingError(this ILogger logger, string entityName, Exception? exception = null)
	{
		DatabaseSavingErrorExecute(logger, entityName, exception);
	}

	private static readonly Action<ILogger, string, Exception?> DatabaseSavingErrorExecute =
		LoggerMessage.Define<string>(LogLevel.Error, EventIdentifiers.DatabaseSavingErrorId, "{entityName}");
}
