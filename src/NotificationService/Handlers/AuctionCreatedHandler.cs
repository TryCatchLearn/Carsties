using Contracts;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Handlers;

public class AuctionCreatedHandler(IHubContext<NotificationHub> hubContext)
{
    public async Task Handle(AuctionCreated message)
    {
        Console.WriteLine($"=== Auction created message received ===");
        
        await hubContext.Clients.All.SendAsync("AuctionCreated", message);
    }
}