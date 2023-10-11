using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface IFactService
{
	IEnumerable<Fact> GetLastTwentyFacts();
}