using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Mappers.Base;

namespace OriGames.Facts.Infrastructure.Mappers;

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