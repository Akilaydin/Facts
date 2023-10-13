using System.Reflection;

using AutoMapper;

using OriGames.Facts.Infrastructure.Mappers.Base;

namespace OriGames.Facts.Infrastructure.Mappers.Configurations;

public static class MapperRegistration
{
	public static MapperConfiguration GetMapperConfiguration()
	{
		var mappingConfigurations = GetMappingConfigurations();

		return new MapperConfiguration(options =>
		{
			foreach (var mappingConfiguration in mappingConfigurations.Select(config => (Profile) Activator.CreateInstance(config)!))
			{
				options.AddProfile(mappingConfiguration);
			}
		});
	}

	private static List<Type> GetMappingConfigurations()
	{
		return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsAssignableTo(typeof(IAutoMapper)) && t.IsAbstract == false).ToList();
	}
}
