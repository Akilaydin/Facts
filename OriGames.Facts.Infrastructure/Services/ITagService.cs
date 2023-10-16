using OriGames.Facts.Domain.Data;
using OriGames.Facts.Domain.Interfaces;

namespace OriGames.Facts.Infrastructure.Services;

public interface ITagService
{
	Task<List<TagCloud>> GetTagCloudAsync();

	Task ProcessTagsAsync(ITagsHolder tagsHolder, Fact fact, CancellationToken token);
}