using System.ComponentModel.DataAnnotations;

using OriGames.Facts.Web.Infrastructure;
using OriGames.Facts.Web.Interfaces;

namespace OriGames.Facts.Web.ViewModels;

public class FactCreateViewModel : ITagsHolder
{
	/// <summary>
	/// Content for editing
	/// </summary>
	[Display(Name = "Содержание факта")]
	[Required]
	[MinLength(10, ErrorMessage = "Факт должен быть длиной хотя бы {1} символов")]
	public string? Content { get; set; }

	public List<string>? Tags { get; set; }

	[Range(1, 8, ErrorMessage = "Требуется от 1 до 8 меток")]
	public int TotalTags { get; set; }
}