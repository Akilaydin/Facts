using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OriGames.Facts.RazorLibrary;

public class TagEditorComponentModel: ComponentBase
{
	[Parameter]
	public List<string> Tags { get; set; }

	[Inject] 
	public IJSRuntime JsRuntime { get; set; }

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
}
