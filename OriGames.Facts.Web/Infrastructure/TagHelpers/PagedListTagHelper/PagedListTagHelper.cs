using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OriGames.Facts.Web.Infrastructure.TagHelpers.PagedListTagHelper;

[HtmlTargetElement("pager", Attributes = PagerListPageIndexAttributeName)]
[HtmlTargetElement("pager", Attributes = PagerListPageSizeAttributeName)]
[HtmlTargetElement("pager", Attributes = PagerListTotalCountAttributeName)]
[HtmlTargetElement("pager", Attributes = PagerListActionAttributeName)]
[HtmlTargetElement("pager", Attributes = HostAttributeName)]
[HtmlTargetElement("pager", Attributes = FragmentAttributeName)]
[HtmlTargetElement("pager", Attributes = PagedListRouteAttributeName)]
[HtmlTargetElement("pager", Attributes = PagedListRouteDataAttributeName)]
[HtmlTargetElement("pager", Attributes = ProtocolAttributeName)]
[HtmlTargetElement("pager", Attributes = ControllerAttributeName)]
public class PagedListTagHelper : TagHelper
{
	private readonly IHtmlGenerator _htmlGenerator;
	private readonly IDictionary<string, string> _routeValues = new Dictionary<string, string>();

	private const string PagerListPageIndexAttributeName = "asp-paged-list-page-index";
	private const string PagerListPageSizeAttributeName = "asp-paged-list-page-size";
	private const string PagerListTotalCountAttributeName = "asp-paged-list-total-count";
	private const string PagerListActionAttributeName = "asp-paged-list-url";
	private const string HostAttributeName = "asp-host";
	private const string FragmentAttributeName = "asp-fragment";
	private const string PagedListRouteAttributeName = "asp-route-parameter";
	private const string PagedListRouteDataAttributeName = "asp-route-data";
	private const string ProtocolAttributeName = "asp-protocol";
	private const string ControllerAttributeName = "asp-controller";

	#region Properties for Attributes
	[HtmlAttributeName(PagerListPageIndexAttributeName)] public int PagedListIndex { get; set; }

	[HtmlAttributeName(PagerListPageSizeAttributeName)] public int PagedListSize { get; set; }

	[HtmlAttributeName(PagerListTotalCountAttributeName)] public int PagedListTotalCount { get; set; }
	#endregion

	#region Properties for Private calculation
	private string DisableCss => "disabled";

	private string PageLinkCss => "page-link";

	private string RootTagCss => "pagination";

	private string ActiveTagCss => "active";

	private string PageItemCss => "page-item";

	private byte VisibleGroupCount => 10;
	#endregion

	#region Properties for Generator
	[ViewContext] public ViewContext? ViewContext { get; set; }

	[HtmlAttributeName(PagerListActionAttributeName)] public string? ActionName { get; set; }

	[HtmlAttributeName(PagedListRouteAttributeName)] public string RouteParameter { get; set; } = null!;

	[HtmlAttributeName(PagedListRouteDataAttributeName)] public object? RouteParameters { get; set; }

	/// <summary>
	/// The URL fragment name.
	/// </summary>
	[HtmlAttributeName(FragmentAttributeName)]
	public string? Fragment { get; set; }

	/// <summary>
	/// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
	/// </summary>
	[HtmlAttributeName(ProtocolAttributeName)]
	public string? Protocol { get; set; }

	/// <summary>
	/// The host name.
	/// </summary>
	[HtmlAttributeName(HostAttributeName)]
	public string? Host { get; set; }

	/// <summary>
	/// The name of the controller.
	/// </summary>
	/// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
	[HtmlAttributeName(ControllerAttributeName)]
	public string Controller { get; set; } = null!;
	#endregion

	private readonly IPagerTagHelperService _pagerTagHelperService;

	public PagedListTagHelper(IPagerTagHelperService pagerTagHelperService, IHtmlGenerator htmlGenerator)
	{
		_pagerTagHelperService = pagerTagHelperService;
		_htmlGenerator = htmlGenerator;
	}

	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		if (PagedListTotalCount <= 1)
		{
			return;
		}

		var pagerContext = _pagerTagHelperService.GetPagerContext(PagedListIndex, PagedListSize, PagedListTotalCount, 10);

		var pages = _pagerTagHelperService.GetPages(pagerContext);

		var ulTagBuilder = new TagBuilder("ul");
		ulTagBuilder.AddCssClass(RootTagCss);

		foreach (var page in pages)
		{
			var liTagBuilder = new TagBuilder("li");
			liTagBuilder.AddCssClass(PageItemCss);

			if (page.IsActive)
			{
				liTagBuilder.AddCssClass(ActiveTagCss);
			}

			if (page.IsDisabled)
			{
				liTagBuilder.AddCssClass(DisableCss);
			}

			liTagBuilder.InnerHtml.AppendHtml(GenerateLink(page.Title, page.Value.ToString()));
			ulTagBuilder.InnerHtml.AppendHtml(liTagBuilder);
		}

		output.Content.AppendHtml(ulTagBuilder);

		base.Process(context, output);
	}

	private TagBuilder GenerateLink(string linkText, string routeValue)
	{
		var routeValues = new RouteValueDictionary(_routeValues!) { { RouteParameter, routeValue } };
		
		if (RouteParameters != null)
		{
			var values = RouteParameters.GetType().GetProperties();
			
			if (values.Any())
			{
				foreach (var propertyInfo in values)
				{
					routeValues.Add(propertyInfo.Name, propertyInfo.GetValue(RouteParameters));
				}
			}
		}

		return _htmlGenerator.GenerateActionLink(ViewContext, actionName: ActionName, controllerName: Controller, routeValues: routeValues, hostname: Host, linkText: linkText,
			fragment: Fragment, htmlAttributes: new { @class = PageLinkCss }, protocol: Protocol);
	}
}
