namespace OriGames.Facts.Web.TagHelpers.PagedListTagHelper.Base;

public abstract class PagerPageBase
{
	public string Title { get; }

	public int Value { get; }

	public bool IsActive { get; }

	public bool IsDisabled { get; }
	
	protected PagerPageBase(string title, int value, bool isActive = false, bool isDisabled = false)
	{
		Title = title;
		Value = value;
		IsActive = isActive;
		IsDisabled = isDisabled;
	}
}
