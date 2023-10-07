﻿using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Mappers.Base;

namespace OriGames.Facts.Web.Infrastructure.Mappers;

public class NotificationMapperConfiguration : MapperConfigurationBase
{
	public NotificationMapperConfiguration()
	{
		CreateMap<Notification, EmailMessage>()
			.ForMember(n => n.Author, o => o.MapFrom(e => e.CreatedBy))
			.ForMember(n => n.Recipient, o => o.Ignore())
			.ForMember(n => n.Body, o => o.MapFrom(e => e.Content))
			.ForMember(n => n.IsHtml, o => o.MapFrom(e => true));
	}
}
