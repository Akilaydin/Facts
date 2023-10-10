using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Domain.Data;

public class Tag : Identity
{
	public string Name { get; set; } = null!;

	public ICollection<Fact> Facts { get; set; } = new List<Fact>();
}
