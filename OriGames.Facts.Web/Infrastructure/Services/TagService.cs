using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Services;

public class TagService : ITagService
{
	private readonly IUnitOfWork _unitOfWork;

	public TagService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<List<TagCloud>> GetTagCloudAsync()
	{
		var tags = await _unitOfWork.GetRepository<Tag>().GetAll(true)
			.Select(s => new TagCloud { Name = s.Name, Id = s.Id, CssClass = "", Total = s.Facts == null ? 0 : s.Facts!.Count }).ToListAsync();

		var cloud = TagCloudHelper.Generate(tags, 10);

		return cloud;
	}
}