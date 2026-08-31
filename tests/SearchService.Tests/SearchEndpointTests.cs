using System.Net;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Models;

namespace SearchService.Tests;

[Collection("search-service")]
public class SearchEndpointTests(AppFixture fixture) : IAsyncLifetime
{
    private readonly Item _item1 = TestData.CreateItem(x =>
    {
        x.Make = "Alpha";
        x.Model = "Orion";
        x.Winner = "tom";
    });
    
    private readonly Item _item2 = TestData.CreateItem(x =>
    {
        x.Make = "Alpha";
        x.Model = "Beta";
    });
    
    private readonly Item _item3 = TestData.CreateItem(x =>
    {
        x.Make = "Zeta";
        x.Model = "Delta";
        x.Seller = "bob";
    });
    
    public async Task InitializeAsync()
    {
        var client = fixture.Host.Services.GetRequiredService<MeilisearchClient>();
        
        var addTask = await client.Index("items")
            .AddDocumentsAsync([_item1, _item2, _item3], primaryKey: "id");
        
        await client.WaitForTaskAsync(addTask.TaskUid);
    }

    public async Task DisposeAsync()
    {
        var client = fixture.Host.Services.GetRequiredService<MeilisearchClient>();
        
        var deleteTask = await client.Index("items").DeleteAllDocumentsAsync();
        await client.WaitForTaskAsync(deleteTask.TaskUid);
    }

    private record SearchResponse(List<Item> Results, int PageCount, int TotalCount);

    [Fact]
    public async Task GetSearchResults_WithSearchTerm_ReturnsMatchingItem()
    {
        var result = await fixture.Host.Scenario(s =>
        {
            s.Get.Url("/api/search?searchTerm=zet");
            s.StatusCodeShouldBe(HttpStatusCode.OK);
        });
        
        var response = await result.ReadAsJsonAsync<SearchResponse>();
        
        var item = Assert.Single(response.Results);
        Assert.Equal(_item3.Id, item.Id);
    }
    
    [Fact]
    public async Task GetSearchResults_FilteredByWinner_ReturnsWonItem()
    {
        var result = await fixture.Host.Scenario(s =>
        {
            s.Get.Url("/api/search?winner=tom");
            s.StatusCodeShouldBe(HttpStatusCode.OK);
        });
        
        var response = await result.ReadAsJsonAsync<SearchResponse>();
        
        var item = Assert.Single(response.Results);
        Assert.Equal(_item1.Id, item.Id);
    }
    
    [Fact]
    public async Task GetSearchResults_WithPagination_ReturnsCorrectPage()
    {
        var result = await fixture.Host.Scenario(s =>
        {
            s.Get.Url("/api/search?orderBy=make&pageSize=1&pageNumber=2");
            s.StatusCodeShouldBe(HttpStatusCode.OK);
        });
        
        var response = await result.ReadAsJsonAsync<SearchResponse>();
        
        var item = Assert.Single(response.Results);
        Assert.Equal(_item1.Id, item.Id);
    }
}