using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OriGames.Facts.Domain.Data;

namespace OriGames.Facts.Infrastructure.Data.Configurations;

public class NotificationModelConfiguration : IEntityTypeConfiguration<Notification>
{
	void IEntityTypeConfiguration<Notification>.Configure(EntityTypeBuilder<Notification> builder)
	{
		builder.ToTable("Notifications");

		builder.HasKey(n => n.Id);
		
		builder.Property(n => n.Id);
		
		builder.Property(n => n.CreatedAt).IsRequired();
		builder.Property(n => n.CreatedBy).IsRequired().HasMaxLength(100);
		builder.Property(n => n.UpdatedAt);
		builder.Property(n => n.UpdatedBy).HasMaxLength(100);
		
		builder.Property(n => n.Subject).HasMaxLength(1000).IsRequired();
		builder.Property(n => n.Content).HasMaxLength(4000).IsRequired();

		builder.Property(n => n.From).HasMaxLength(128).IsRequired();
		builder.Property(n => n.To).HasMaxLength(128).IsRequired();

		builder.Property(n => n.IsSent);

		builder.HasIndex(n => n.From);
		builder.HasIndex(n => n.To);
		builder.HasIndex(n => n.Subject);
		builder.HasIndex(n => n.Content);
	}
}
