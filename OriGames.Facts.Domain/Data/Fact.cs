using Calabonga.EntityFrameworkCore.Entities.Base;

namespace OriGames.Facts.Domain.Data;

public class Fact : Auditable
{
	public string Content { get; set; }
	
	public ICollection<Tag>? Tags { get; set; }
}
