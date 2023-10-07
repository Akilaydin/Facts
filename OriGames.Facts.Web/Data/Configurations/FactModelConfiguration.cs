using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OriGames.Facts.Web.Data.Configurations;

public class FactModelConfiguration : IEntityTypeConfiguration<Fact>
{
	void IEntityTypeConfiguration<Fact>.Configure(EntityTypeBuilder<Fact> builder)
	{
		builder.ToTable("Facts");
		builder.HasKey(p => p.Id);
		
		builder.Property(p => p.Id);
		builder.Property(p => p.Content).HasMaxLength(3000).IsRequired();
		builder.Property(p => p.CreatedAt).IsRequired();
		builder.Property(p => p.CreatedBy).IsRequired().HasMaxLength(100);
		builder.Property(p => p.UpdatedAt);
		builder.Property(p => p.UpdatedBy).HasMaxLength(100);
		
		builder.HasMany(p => p.Tags).WithMany(p => p.Facts);
	}
}
