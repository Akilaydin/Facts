using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Mappers.Base;
using OriGames.Facts.Infrastructure.Mappers.Converters;

namespace OriGames.Facts.Infrastructure.Mappers;

public class FactMapperConfiguration : MapperConfigurationBase
{
	public FactMapperConfiguration()
	{
		CreateMap<Fact, FactViewModel>();
		
		CreateMap<FactCreateViewModel, Fact>()
			.ForMember(m => m.Id, o=> o.Ignore())
			.ForMember(m => m.Tags, o=> o.Ignore())
			.ForMember(m => m.CreatedAt, o=> o.Ignore())
			.ForMember(m => m.CreatedBy, o=> o.Ignore())
			.ForMember(m => m.UpdatedAt, o=> o.Ignore())
			.ForMember(m => m.UpdatedBy, o=> o.Ignore())
			;

		CreateMap<Fact, FactEditViewModel>()
			.ForMember(m => m.TotalTags, o => o.Ignore())
			.ForMember(m => m.ReturnUrl, o => o.Ignore());
		
		CreateMap<FactEditViewModel, Fact>()
			.ForMember(m => m.Id, o=> o.Ignore())
			.ForMember(m => m.Tags, o=> o.Ignore())
			.ForMember(m => m.CreatedAt, o=> o.Ignore())
			.ForMember(m => m.CreatedBy, o=> o.Ignore())
			.ForMember(m => m.UpdatedAt, o=> o.Ignore())
			.ForMember(m => m.UpdatedBy, o=> o.Ignore())
			;

		CreateMap<IPagedList<Fact>, IPagedList<FactViewModel>>().ConvertUsing<PagedListConverter<Fact, FactViewModel>>();
	}
}