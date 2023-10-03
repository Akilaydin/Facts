using System.Text;
using System.Xml;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;

using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

using OriGames.Facts.Web.Infrastructure.Services;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public record FactRssRequest : RequestBase<string>;

public class FactRssRequestHandler : RequestHandlerBase<FactRssRequest, string>
{
	private readonly IFactService _factService;

	public FactRssRequestHandler(IFactService factService)
	{
		_factService = factService;
	}

	public override async Task<string> Handle(FactRssRequest request, CancellationToken cancellationToken)
	{
		await using var sw = new EncodingStringWriterService(Encoding.UTF8);
		await using var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings {
			Async = true,
			Indent = true
		});
		var writer = new RssFeedWriter(xmlWriter);

		await writer.WriteTitle(".NET Programming");
		await writer.WriteDescription("RSS 2.0!");
		await writer.Write(new SyndicationLink(new Uri("https://www.google.com/")));
		await writer.Write(new SyndicationPerson("TestName", "TestEmail@gmail.com", RssContributorTypes.ManagingEditor));
		await writer.WritePubDate(DateTimeOffset.Now);

		var posts = _factService.GetLastTwentyFacts();

		foreach (var post in posts)
		{
			var factTheme = post.Tags!.MinBy(_ => Guid.NewGuid());

			var item = new SyndicationItem {
				Id = post.Id.ToString(),
				Title = $"Факт на тему \"{factTheme.Name}\"",
				Description = post.Content,
				Published = post.CreatedAt,
				LastUpdated = post.UpdatedAt ??= post.CreatedAt
			};

			foreach (var tag in post.Tags!)
			{
				item.AddCategory(new SyndicationCategory($"{tag.Name}"));
			}

			await writer.Write(item);
		}

		await writer.WriteRaw("</channel>");
		await writer.WriteRaw("</rss>");
		await xmlWriter.FlushAsync();

		return sw.ToString();
	}
}
