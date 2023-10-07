using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface ITagService
{
	Task<List<TagCloud>> GetTagCloudAsync();

	Task ProcessTagsAsync(ITagsHolder tagsHolder, Fact fact, CancellationToken token);
}