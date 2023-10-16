using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using OriGames.Facts.Infrastructure.Services;

namespace OriGames.Facts.Web.Razor;

public class TagEditorComponentModel : ComponentBase
{
	[Parameter] public List<string>? Tags { get; set; }

	[Inject] public IJSRuntime JsRuntime { get; set; }
	
	[Inject] public ITagSearchService TagSearchService { get; set; }
	
	protected List<string>? FoundTags { get; set; }
	
	protected string TagName { get; set; }

	protected async Task DeleteTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return;
		}

		var tagToDelete = Tags.SingleOrDefault(x => x == tag);
		if (tagToDelete is null)
		{
			return;
		}

		Tags.Remove(tag);

		await new RazorInterop(JsRuntime).SetTagTotal(Tags.Count);
	}

	protected void SearchTags(ChangeEventArgs eventArgs)
	{
		if (eventArgs?.Value == null)
		{
			FoundTags = null;
			return;
		}

		if (string.IsNullOrEmpty(eventArgs.Value.ToString()))
		{
			FoundTags = null;
			return;
		}

		FoundTags = TagSearchService.SearchTags(eventArgs.Value.ToString());
	}
	
	protected async Task AddTag(string? tag)
	{
		var formalizedTag = tag?.ToLower().Trim();
		
		if (string.IsNullOrEmpty(formalizedTag))
		{
			return;
		}

		Tags ??= new List<string>();

		if (Tags.Exists(t => t.Equals(tag, StringComparison.InvariantCulture)) == false)
		{
			Tags.Add(formalizedTag);

			await new RazorInterop(JsRuntime).SetTagTotal(Tags.Count);
		}

		TagName = string.Empty;
		FoundTags = null;
	}
}
