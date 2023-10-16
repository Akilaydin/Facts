namespace OriGames.Facts.Infrastructure.Services;

public interface ITagSearchService
{
	List<string> SearchTags(string term);
}