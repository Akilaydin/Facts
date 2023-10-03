using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Services;

public interface ITagService
{
	Task<List<TagCloud>> GetTagCloudAsync();
}