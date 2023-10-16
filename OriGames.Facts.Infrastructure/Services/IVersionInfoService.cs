namespace OriGames.Facts.Infrastructure.Services;

public interface IVersionInfoService
{
	string Version { get; }

	string Branch { get; }

	string Commit { get; }
}
