using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface IFactService
{
	IEnumerable<Fact> GetLastTwentyFacts();
}