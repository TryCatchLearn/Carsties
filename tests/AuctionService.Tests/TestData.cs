using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Tests;

internal static class TestData
{
    public static Auction CreateAuction(Action<Auction>? configure = null)
    {
        var auction = new Auction
        {
            Id = Guid.NewGuid().ToString(),
            Seller = "seller",
            ReservePrice = 100,
            Status = Status.Live,
            Item = new Item
            {
                Make = "Ford",
                Model = "Mustang",
                Year = 2021,
                Mileage = 1000,
                Color = "Red",
                Description = "Test car",
                ImageUrl = "https://image.url"
            }
        };
        
        configure?.Invoke(auction);
        
        return auction;
    }

    public static CreateAuctionDto BuildCreateAuctionDto(Action<CreateAuctionDto>? configure = null)
    {
        var dto = new CreateAuctionDto
        {
            Make = "Ford",
            Model = "Mustang",
            Year = 2021,
            Mileage = 1000,
            Color = "Red",
            Description = "Test car",
            ImageUrl = "https://image.url",
            AuctionEnd = DateTime.UtcNow.AddDays(7),
        };
        
        configure?.Invoke(dto);
        
        return dto;
    }
}