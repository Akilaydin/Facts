using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Mappers.Base;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Mappers;

public class TagMapperConfiguration : MapperConfigurationBase
{
	public TagMapperConfiguration()
	{
		CreateMap<Tag, TagViewModel>();
		
		CreateMap<Tag, TagUpdateViewModel>();
		CreateMap<TagUpdateViewModel, Tag>()
			.ForMember(m => m.Id, o => o.Ignore())
			.ForMember(m => m.Facts, o => o.Ignore())
			;
	}
}