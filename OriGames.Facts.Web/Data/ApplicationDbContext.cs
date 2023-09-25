using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Web.Data.Base;

namespace OriGames.Facts.Web.Data;

public class ApplicationDbContext : BaseDbContext
{
	public DbSet<Fact> Facts { get; set; }
	public DbSet<Tag> Tags { get; set; }
	
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
		
	}
}