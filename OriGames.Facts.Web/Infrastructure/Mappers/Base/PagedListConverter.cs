using AutoMapper;

using Calabonga.UnitOfWork;

namespace OriGames.Facts.Web.Infrastructure.Mappers.Base;

public class PagedListConverter<TSource, TDestination> : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
{
	public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination, ResolutionContext context)
	{
		return source == null 
			? PagedList.Empty<TDestination>()
			: PagedList.From(source, converter => context.Mapper.Map<IEnumerable<TDestination>>(converter));
	}
}