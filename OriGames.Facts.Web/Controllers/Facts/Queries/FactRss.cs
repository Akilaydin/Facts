using System.Text;
using System.Xml;

using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;

using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

using OriGames.Facts.Web.Data;
using OriGames.Facts.Web.Infrastructure.Services;

namespace OriGames.Facts.Web.Controllers.Facts.Queries;

public record FactRssRequest : RequestBase<string>;

public class FactRssRequestHandler : RequestHandlerBase<FactRssRequest, string>
{
	private readonly IFactService _factService;

	public FactRssRequestHandler(IFactService factService) {
		_factService = factService;
	}

	public override async Task<string> Handle(FactRssRequest request, CancellationToken cancellationToken)
	{
		await using var sw = new EncodingStringWriterService(Encoding.UTF8);
		await using XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings
		{
			Async = true,
			Indent = true
		});
		var writer = new RssFeedWriter(xmlWriter);

		await writer.WriteTitle(".NET Programming");
		await writer.WriteDescription("RSS 2.0!");
		await writer.Write(new SyndicationLink(new Uri("https://www.calabonga.net")));
		await writer.Write(new SyndicationPerson("Calabonga", "dev@calabonga.net (Sergei Calabonga)", RssContributorTypes.ManagingEditor));
		await writer.WritePubDate(DateTimeOffset.Now);

		var posts = _factService.GetTwentyFacts();
		foreach (var post in posts)
		{
			var newTag = new Tag() { Name = "No tag" };

			if (post.Tags.Count == 0)
			{
				post.Tags.Add(newTag);
			}
            
			var item = new SyndicationItem
			{
				Id = post.Id.ToString(),
				Title = $"Факт на тему \"{post.Tags!.OrderBy(_ => Guid.NewGuid()).First().Name}\"",
				Description = post.Content,
				Published = post.CreatedAt,
				LastUpdated = post.UpdatedAt ??= post.CreatedAt
			};

			item.AddLink(new SyndicationLink(new Uri($"https://www.calabonga.net/blog/post/{post.Id}")));
			foreach (var tag in post.Tags!)
			{
				item.AddCategory(new SyndicationCategory($"{tag.Name}"));
			}

			item.AddContributor(new SyndicationPerson("Calabonga", "dev@calabonga.net (Sergei Calabonga)"));

			await writer.Write(item);


		}
		await writer.WriteRaw("</channel>");
		await writer.WriteRaw("</rss>");
		await xmlWriter.FlushAsync();
		return sw.ToString();
	}
}
