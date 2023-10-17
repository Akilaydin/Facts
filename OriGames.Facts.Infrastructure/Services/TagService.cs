using System.Collections.ObjectModel;

using Calabonga.UnitOfWork;

using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Domain.Interfaces;

namespace OriGames.Facts.Infrastructure.Services;

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

		var cloud = GenerateTagCloud(tags, 10);

		return cloud;
	}

	async Task ITagService.ProcessTagsAsync(ITagsHolder? tagsHolder, Fact fact, CancellationToken cancellationToken)
	{
		if (tagsHolder?.Tags is null)
		{
			throw new ArgumentNullException(nameof(tagsHolder));
		}

		if (fact == null)
		{
			throw new ArgumentNullException(nameof(fact));
		}

		var tagsRepository = _unitOfWork.GetRepository<Tag>();
		var factsRepository = _unitOfWork.GetRepository<Fact>();

		var tagsAfterEdit = tagsHolder.Tags!.ToArray();
		var tagsBeforeEdit = Enumerable.ToArray<string>(tagsRepository.GetAll(x => x.Name.ToLower(), x => Enumerable.Select(x.Facts!, p => p.Id).Contains(fact.Id), null));

		var (tagsToCreate, tagsToDelete) = FindDifferenceInTags(tagsBeforeEdit, tagsAfterEdit);

		if (tagsToDelete.Any())
		{
			foreach (var tagToDelete in tagsToDelete)
			{
				var tag = await tagsRepository.GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == tagToDelete, disableTracking: false);
				if (tag == null)
				{
					continue;
				}

				fact.Tags.Remove(tag);

				var timesUsed = factsRepository.GetAll(predicate: x => x.Tags!.Select(t => t.Name).Contains(tag.Name)).Count();
				
				if (timesUsed == 1)
				{
					tagsRepository.Delete(tag);
				}
			}
		}

		fact.Tags ??= new Collection<Tag>();

		foreach (var name in tagsToCreate)
		{
			var tag = await tagsRepository.GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower() == name, disableTracking: false);
			if (tag == null)
			{
				var t = new Tag {
					Name = name.Trim().ToLower()
				};
				
				await tagsRepository.InsertAsync(t, cancellationToken);
				
				fact.Tags!.Add(t);
			}
			else
			{
				fact.Tags!.Add(tag);
			}
		}
	}

	private static List<TagCloud> GenerateTagCloud(List<TagCloud> items, int clusterCount)
	{
		var totalCount = items.Count;
		var tagsCloud = items.OrderBy(x => x.Total).ToList();

		var clusters = new List<List<TagCloud>>();
		if (totalCount > 0)
		{
			var min = tagsCloud.Min(c => c.Total);
			var max = tagsCloud.Max(c => c.Total) + min;
			var completeRange = max - min;
			var groupRange = completeRange / (double)clusterCount;
			var cluster = new List<TagCloud>();
			var currentRange = min + groupRange;
			for (var i = 0; i < totalCount; i++)
			{
				while (tagsCloud.ToArray()[i].Total > currentRange)
				{
					clusters.Add(cluster);
					cluster = new List<TagCloud>();
					currentRange += groupRange;
				}
				cluster.Add(tagsCloud.ToArray()[i]);
			}
			clusters.Add(cluster);
		}
		var result = new List<TagCloud>();
		for (var i = 0; i < clusters.Count; i++)
		{
			result.AddRange(clusters[i].Select(item => new TagCloud {
				Id = item.Id,
				Name = item.Name,
				CssClass = "tag" + i,
				Total = item.Total
			}));
		}

		return result.OrderBy(x => x.Name).ToList();
	}
	
	private static (string[] toCreate, string[] toDelete) FindDifferenceInTags(string[] old, string[] current)
	{
		var mask = current.Intersect(old);
		
		var toDelete = old.Except(current).ToArray();
		
		var toCreate = current.Except(mask).ToArray();
		
		return new(toCreate, toDelete);
	}
}
