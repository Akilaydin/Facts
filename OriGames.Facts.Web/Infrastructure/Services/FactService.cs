using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;

namespace OriGames.Facts.Web.Infrastructure.Services;

public class FactService : IFactService
{
	private readonly IUnitOfWork _unitOfWork;

	public FactService(IUnitOfWork unitOfWork) {
		_unitOfWork = unitOfWork;
	}

	public IEnumerable<Fact> GetLastTwentyFacts()
	{
		return _unitOfWork.GetRepository<Fact>()
			.GetAll(true)
			.Include(x => x.Tags)
			.OrderByDescending(f => f.CreatedAt)
			.Take(20)
			.AsEnumerable();
	}
}
