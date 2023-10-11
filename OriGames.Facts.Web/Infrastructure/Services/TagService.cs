using System.Collections.ObjectModel;

using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Web.Infrastructure.Helpers;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Services;

public class TagService : ITagService
{
	private readonly IUnitOfWork _unitOfWork;

	public TagService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	async Task<List<TagCloud>> ITagService.GetTagCloudAsync()
	{
		var tags = await _unitOfWork.GetRepository<Tag>().GetAll(true)
			.Select(s => new TagCloud { Name = s.Name, Id = s.Id, CssClass = "", Total = s.Facts == null ? 0 : s.Facts!.Count }).ToListAsync();

		var cloud = FactHelper.GenerateTagCloud(tags, 10);

		return cloud;
	}

	public async Task ProcessTagsAsync(ITagsHolder? tagsHolder, Fact fact, CancellationToken cancellationToken)
	{
		if (tagsHolder?.Tags is null)
		{
			throw new ArgumentNullException(nameof(tagsHolder));
		}

		if (fact == null)
		{
			throw new ArgumentNullException(nameof(fact));
		}

		var tagRepository = _unitOfWork.GetRepository<Tag>();

		var tagsAfterEdit = tagsHolder.Tags!.ToArray();
		var tagsBeforeEdit = tagRepository.GetAll(x => x.Name.ToLower(), x => x.Facts!.Select(p => p.Id).Contains(fact.Id), null).ToArray();

		var (tagsToCreate, tagsToDelete) = FactHelper.FindDifferenceInTags(tagsBeforeEdit, tagsAfterEdit);

		if (tagsToDelete.Any())
		{
			foreach (var name in tagsToDelete)
			{
				var tag = await tagRepository.GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == name, disableTracking: false);
				if (tag == null)
				{
					continue;
				}

				var used = _unitOfWork.GetRepository<Fact>().GetAll(x => x.Tags!.Select(t => t.Name).Contains(tag.Name), true).ToArray();

				if (used.Length == 1)
				{
					tagRepository.Delete(tag);
				}
			}
		}

		fact.Tags ??= new Collection<Tag>();

		foreach (var name in tagsToCreate)
		{
			var tag = await tagRepository.GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == name, disableTracking: false);
			if (tag == null)
			{
				var t = new Tag {
					Name = name.Trim().ToLower()
				};
				
				await tagRepository.InsertAsync(t, cancellationToken);
				
				fact.Tags!.Add(t);
			}
			else
			{
				fact.Tags!.Add(tag);
			}
		}
	}
}
