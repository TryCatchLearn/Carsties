using Contracts;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Handlers;

public class BidPlacedHandler(IHubContext<NotificationHub> hubContext)
{
    public async Task Handle(BidPlaced message)
    {
        Console.WriteLine($"=== Bid placed message received ===");
        
        await hubContext.Clients.All.SendAsync("BidPlaced", message);
    }
}