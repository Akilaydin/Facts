using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Infrastructure.Services;

public interface IFactService
{
	IEnumerable<Fact> GetLastTwentyFacts();
}