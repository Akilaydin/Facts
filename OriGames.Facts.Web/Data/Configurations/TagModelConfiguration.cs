using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OriGames.Facts.Web.Data.Configurations;

public class TagModelConfiguration : IEntityTypeConfiguration<Tag>
{

	void IEntityTypeConfiguration<Tag>.Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.ToTable("Tags");
		builder.HasKey(p => p.Id);
		
		builder.Property(p => p.Id);
		builder.Property(p => p.Name).HasMaxLength(100);

		builder.HasIndex(i => i.Name);
	}
}
