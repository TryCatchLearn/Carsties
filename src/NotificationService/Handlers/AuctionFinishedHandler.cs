using Contracts;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Handlers;

public class AuctionFinishedHandler(IHubContext<NotificationHub> hubContext)
{
    public async Task Handle(AuctionFinished message)
    {
        Console.WriteLine($"=== Auction finished message received ===");
        
        await hubContext.Clients.All.SendAsync("AuctionFinished", message);
    }
}