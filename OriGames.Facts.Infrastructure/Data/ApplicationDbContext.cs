using Microsoft.EntityFrameworkCore;

using OriGames.Facts.Domain.Data;
using OriGames.Facts.Infrastructure.Data.Base;

namespace OriGames.Facts.Infrastructure.Data;

public class ApplicationDbContext : BaseDbContext
{
	public DbSet<Fact> Facts { get; set; } = null!;

	public DbSet<Tag> Tags { get; set; } = null!;
	
	public DbSet<Notification> Notifications { get; set; } = null!;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}