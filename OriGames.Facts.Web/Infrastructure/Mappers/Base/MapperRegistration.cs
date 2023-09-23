using AutoMapper;

namespace OriGames.Facts.Web.Infrastructure.Mappers.Base;

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
		return typeof(Program).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IAutoMapper)) && t.IsAbstract == false).ToList();
	}
}
