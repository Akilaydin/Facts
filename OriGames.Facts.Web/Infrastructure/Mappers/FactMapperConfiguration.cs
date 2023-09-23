using Calabonga.UnitOfWork;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Mappers.Base;
using OriGames.Facts.Web.ViewModels;

namespace OriGames.Facts.Web.Infrastructure.Mappers;

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
		
		CreateMap<Fact, FactUpdateViewModel>();
		
		CreateMap<FactUpdateViewModel, Fact>()
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