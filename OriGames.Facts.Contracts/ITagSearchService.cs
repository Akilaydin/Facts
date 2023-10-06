namespace OriGames.Facts.Contracts;

public interface ITagSearchService
{
	List<string> SearchTags(string term);
}