using AuctionService.Data;
using Contracts;

namespace AuctionService.Handlers;

public class BidPlacedHandler
{
    public async Task Handle(BidPlaced message, AuctionDbContext dbContext)
    {
        Console.WriteLine($"Executed {nameof(BidPlacedHandler)} for {message.Amount} for auction id of {message.AuctionId}");
        
        var auction = await dbContext.Auctions.FindAsync(message.AuctionId)
            ?? throw new InvalidOperationException("Auction not found");

        if (auction.CurrentHighBid == null && message.BidStatus.Contains("Accepted") ||
            message.BidStatus.Contains("Accepted") && message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = message.Amount;
            await dbContext.SaveChangesAsync();
        }
    }
}