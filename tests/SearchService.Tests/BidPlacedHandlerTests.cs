using Contracts;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Models;
using Wolverine.Tracking;

namespace SearchService.Tests;

[Collection("search-service")]
public class BidPlacedHandlerTests(AppFixture fixture) : IAsyncLifetime
{
    private readonly Item _item = TestData.CreateItem();
    
    public async Task InitializeAsync()
    {
        var client = fixture.Host.Services.GetRequiredService<MeilisearchClient>();
        
        var addTask = await client.Index("items")
            .AddDocumentsAsync([_item], primaryKey: "id");
        
        await client.WaitForTaskAsync(addTask.TaskUid);
    }

    public async Task DisposeAsync()
    {
        var client = fixture.Host.Services.GetRequiredService<MeilisearchClient>();
        
        var deleteTask = await client.Index("items").DeleteAllDocumentsAsync();
        await client.WaitForTaskAsync(deleteTask.TaskUid);
    }

    [Fact]
    public async Task Handle_WhenAcceptedBudHigherThanCurrent_UpdatesCurrentBid()
    {
        var message = new BidPlaced
        {
            Id = Guid.NewGuid().ToString(),
            AuctionId = _item.Id,
            Bidder = "bob",
            BidTime = DateTime.UtcNow,
            Amount = 150,
            BidStatus = "Accepted"
        };

        await fixture.Host.InvokeMessageAndWaitAsync(message);
        
        var client = fixture.Host.Services.GetRequiredService<MeilisearchClient>();
        
        var item = await client.Index("items").GetDocumentAsync<Item>(_item.Id);
        
        Assert.Equal(150, item.CurrentHighBid);
    }
}