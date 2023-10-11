using AutoMapper;

namespace OriGames.Facts.Infrastructure.Mappers.Converters;

public class PagedListConverter<TSource, TDestination> : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
{
	IPagedList<TDestination> ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>.Convert(IPagedList<TSource> source, IPagedList<TDestination> destination, ResolutionContext context)
	{
		return source == null 
			? PagedList.Empty<TDestination>()
			: PagedList.From(source, converter => context.Mapper.Map<IEnumerable<TDestination>>(converter));
	}
}