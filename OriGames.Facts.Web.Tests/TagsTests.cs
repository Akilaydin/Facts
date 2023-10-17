using OriGames.Facts.Infrastructure.Services;
using OriGames.Facts.Web.ViewModels;

using Xunit;
using Xunit.Abstractions;

namespace OriGames.Facts.Web.Tests;

public class TagsTests
{
	private readonly ITagService _tagService;
	private readonly ITestOutputHelper _testOutputHelper;

	public TagsTests(ITagService tagService, ITestOutputHelper testOutputHelper)
	{
		_tagService = tagService;
		_testOutputHelper = testOutputHelper;
	}
	
	[Fact]
	public async Task ProcessTagsAsync_Should_Throw_On_fact_Argument_Null()
	{
		//Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _tagService.ProcessTagsAsync(new FactCreateViewModel(), null!, CancellationToken.None));
	}

	[Fact]
	public async Task GetTagCloudAsync_Should_Not_Take_More_Than_10_Seconds()
	{
		//Arrange
		var stopwatch = System.Diagnostics.Stopwatch.StartNew();
		
		//Act
		await _tagService.GetTagCloudAsync();
		
		stopwatch.Stop();
		
		_testOutputHelper.WriteLine($"Elapsed: {stopwatch.Elapsed}");
		
		//Assert
		Assert.True(stopwatch.Elapsed.Seconds <= 10);
	}
}
