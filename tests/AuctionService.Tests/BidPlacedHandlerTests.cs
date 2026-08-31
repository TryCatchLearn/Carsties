using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Tracking;

namespace AuctionService.Tests;

[Collection("auction-service")]
public class BidPlacedHandlerTests(AppFixture fixture) : IAsyncLifetime
{
    private readonly Auction _auctionWithoutBids = TestData.CreateAuction();
    private readonly Auction _auctionWithBids = TestData.CreateAuction(x => x.CurrentHighBid = 100);

    public async Task InitializeAsync()
    {
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

        dbContext.Auctions.Add(_auctionWithoutBids);
        dbContext.Auctions.Add(_auctionWithBids);

        await dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

        dbContext.Auctions.RemoveRange(dbContext.Auctions);
        await dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task Handle_WhenNoCurrentHighBid_SetsCurrentHighBid()
    {
        var message = new BidPlaced
        {
            Id = Guid.NewGuid().ToString(),
            AuctionId = _auctionWithoutBids.Id,
            Bidder = "bob",
            BidTime = DateTime.UtcNow,
            Amount = 120,
            BidStatus = "Accepted"
        };
        
        await fixture.Host.InvokeMessageAndWaitAsync(message);
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        var auction = await dbContext.Auctions.FindAsync(_auctionWithoutBids.Id);
        
        Assert.NotNull(auction);
        Assert.Equal(120, auction.CurrentHighBid);
    }
    
    [Fact]
    public async Task Handle_WhenAcceptedBidHigherThanCurrentHighBid_UpdatesCurrentHighBid()
    {
        var message = new BidPlaced
        {
            Id = Guid.NewGuid().ToString(),
            AuctionId = _auctionWithBids.Id,
            Bidder = "bob",
            BidTime = DateTime.UtcNow,
            Amount = 120,
            BidStatus = "Accepted"
        };
        
        await fixture.Host.InvokeMessageAndWaitAsync(message);
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        var auction = await dbContext.Auctions.FindAsync(_auctionWithBids.Id);
        
        Assert.NotNull(auction);
        Assert.Equal(120, auction.CurrentHighBid);
    }
}