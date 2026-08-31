using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Tracking;

namespace AuctionService.Tests;

[Collection("auction-service")]
public class AuctionFinishedHandlerTests(AppFixture fixture) : IAsyncLifetime
{
    private readonly Auction _auction = TestData.CreateAuction();

    public async Task InitializeAsync()
    {
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

        dbContext.Auctions.Add(_auction);

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
    public async Task Handle_WhenItemSoldAboveReserve_SetsWinnerAndFinishedStatus()
    {
        var message = new AuctionFinished
        {
            AuctionId = _auction.Id,
            ItemSold = true,
            Winner = "bob",
            Seller = "seller",
            Amount = 150
        };
        
        await fixture.Host.InvokeMessageAndWaitAsync(message);
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        var auction = await dbContext.Auctions.FindAsync(_auction.Id);
        
        Assert.NotNull(auction);
        Assert.Equal("bob", auction.Winner);
        Assert.Equal(150, auction.SoldAmount);
        Assert.Equal(Status.Finished, auction.Status);
    }
    
    [Fact]
    public async Task Handle_WhenAmountBelowReserve_SetsReserveNotMetStatus()
    {
        var message = new AuctionFinished
        {
            AuctionId = _auction.Id,
            ItemSold = true,
            Winner = "bob",
            Seller = "seller",
            Amount = 50
        };
        
        await fixture.Host.InvokeMessageAndWaitAsync(message);
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        var auction = await dbContext.Auctions.FindAsync(_auction.Id);
        
        Assert.NotNull(auction);
        Assert.Equal(Status.ReserveNotMet, auction.Status);
    }
    
    [Fact]
    public async Task Handle_WhenAuctionNotFound_ThrowsInvalidOperationException()
    {
        var message = new AuctionFinished
        {
            AuctionId = Guid.NewGuid().ToString(),
            ItemSold = false,
            Seller = "seller",
            Amount = 50
        };
        
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            fixture.Host.InvokeMessageAndWaitAsync(message));
    }
}