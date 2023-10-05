using Calabonga.EntityFrameworkCore.Entities.Base;
using Calabonga.UnitOfWork;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OriGames.Facts.Web.Data.Base;

public abstract class BaseDbContext : IdentityDbContext
{
	public SaveChangesResult SaveChangesResult { get; set; }
	
	protected BaseDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
		SaveChangesResult = new SaveChangesResult();
	}
	
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
		base.OnModelCreating(builder);
	}

	public override int SaveChanges()
	{
		ModifyEntities();
		
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		ModifyEntities();
		
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		ModifyEntities();
		
		return base.SaveChangesAsync(cancellationToken);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
	{
		ModifyEntities();
		
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	private void ModifyEntities()
	{
		const string defaultUser = "System";
		
		var currentDate = DateTime.UtcNow;

		var addedEntities = ChangeTracker.Entries().Where(e => e is { State: EntityState.Added, Entity: IAuditable }).ToArray();

		foreach (var addedEntity in addedEntities)
		{
			var createdAt = addedEntity.Property(nameof(IAuditable.CreatedAt)).CurrentValue;
			var updatedAt = addedEntity.Property(nameof(IAuditable.UpdatedAt)).CurrentValue;
			
			var createdBy = addedEntity.Property(nameof(IAuditable.CreatedBy)).CurrentValue;
			var updatedBy = addedEntity.Property(nameof(IAuditable.UpdatedBy)).CurrentValue;
			
			if (string.IsNullOrEmpty(createdBy?.ToString()))
			{
				addedEntity.Property(nameof(IAuditable.CreatedBy)).CurrentValue = defaultUser;
			}
			
			if (string.IsNullOrEmpty(updatedBy?.ToString()))
			{
				addedEntity.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = defaultUser;
			}
			
			if (DateTime.Parse(createdAt?.ToString()!).Year < 1970)
			{
				addedEntity.Property(nameof(IAuditable.CreatedAt)).CurrentValue = currentDate;
			}

			if (updatedAt == null)
			{
				addedEntity.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = currentDate;
			}
			else if (DateTime.Parse(updatedAt.ToString()!).Year < 1970)
			{
				addedEntity.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = currentDate;
			}
		}
		
		SaveChangesResult.AddMessage($"{addedEntities.Length} entities were added");

		var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToArray();

		foreach (var modifiedEntity in modifiedEntities.Where(x => x.GetType().IsAssignableTo(typeof(IAuditable))))
		{
			var propertyEntry = modifiedEntity.Property(nameof(IAuditable.UpdatedBy));
			
			var userName = propertyEntry.CurrentValue ?? defaultUser;
			
			propertyEntry.CurrentValue = userName;

			modifiedEntity.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = currentDate;
		}
		
		SaveChangesResult.AddMessage($"{modifiedEntities.Length} entities were updated");
	}
}
