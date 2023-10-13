using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Mappers.Base;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Mapping;

public class TagViewModelMapperConfiguration : MapperConfigurationBase
{
	public TagViewModelMapperConfiguration()
	{
		CreateMap<Tag, TagViewModel>();
		
		CreateMap<Tag, TagUpdateViewModel>();
		CreateMap<TagUpdateViewModel, Tag>()
			.ForMember(m => m.Id, o => o.Ignore())
			.ForMember(m => m.Facts, o => o.Ignore())
			;
	}
}