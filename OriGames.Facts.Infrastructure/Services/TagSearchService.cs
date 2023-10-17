using Calabonga.UnitOfWork;

using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Infrastructure.Services;

public class TagSearchService : ITagSearchService
{
	private readonly IUnitOfWork _unitOfWork;

	public TagSearchService(IUnitOfWork unitOfWork) {
		_unitOfWork = unitOfWork;
	}

	List<string> ITagSearchService.SearchTags(string term)
	{
		var tags = _unitOfWork.GetRepository<Tag>()
			.GetAll(s => s.Name, x => x.Name.ToLower()
				.StartsWith(term.ToLower()), true).ToList();

		return tags;
	}
}
