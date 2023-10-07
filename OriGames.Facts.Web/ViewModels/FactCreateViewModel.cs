using System.ComponentModel.DataAnnotations;

using OriGames.Facts.Web.Infrastructure;

namespace OriGames.Facts.Web.ViewModels;

public class FactCreateViewModel : ITagsHolder
{
	/// <summary>
	/// Content for editing
	/// </summary>
	[Display(Name = "Содержание факта")]
	public string? Content { get; set; }

	public List<string>? Tags { get; set; }

	[Range(1, 8, ErrorMessage = "Требуется от 1 до 8 меток")]
	public int TotalTags { get; set; }
}