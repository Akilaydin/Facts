using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Mappers.Base;

namespace OriGames.Facts.Infrastructure.Mappers.Configurations;

public class NotificationEmailMapperConfiguration : MapperConfigurationBase
{
	public NotificationEmailMapperConfiguration()
	{
		CreateMap<Notification, EmailMessage>()
			.ForMember(n => n.Author, o => o.MapFrom(e => e.CreatedBy))
			.ForMember(n => n.Recipient, o => o.Ignore())
			.ForMember(n => n.AddressFrom, o => o.MapFrom(e => e.From))
			.ForMember(n => n.AddressTo, o => o.MapFrom(e => e.To))
			.ForMember(n => n.Body, o => o.MapFrom(e => e.Content))
			.ForMember(n => n.IsHtml, o => o.MapFrom(e => true));
	}
}
