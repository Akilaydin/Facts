namespace OriGames.Facts.Domain.Data;

public class TagCloud
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public string CssClass { get; set; }

	public int Total { get; set; }
}