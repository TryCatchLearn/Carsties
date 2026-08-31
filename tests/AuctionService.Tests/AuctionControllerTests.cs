using System.Net;
using System.Security.Claims;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.Tests;

[Collection("auction-service")]
public class AuctionControllerTests(AppFixture fixture) : IAsyncLifetime
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
    public async Task GetAuctions_WithNoParams_ReturnsListOfAuctions()
    {
        var result = await fixture.Host.Scenario(s =>
        {
            s.Get.Url("/api/auctions");
            s.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        var auctions = await result.ReadAsJsonAsync<List<AuctionDto>>();

        var dto = Assert.Single(auctions, x => x.Id == _auction.Id);
        Assert.Equal(_auction.Seller, dto.Seller);
        Assert.Equal(_auction.Item.Make, dto.Make);
        Assert.Equal(_auction.Item.Model, dto.Model);
        Assert.Equal(_auction.Status.ToString(), dto.Status);
    }

    [Fact]
    public async Task GetAuction_WithValidId_ReturnsAuction()
    {
        var result = await fixture.Host.Scenario(s =>
        {
            s.Get.Url($"/api/auctions/{_auction.Id}");
            s.StatusCodeShouldBe(HttpStatusCode.OK);
        });

        var dto = await result.ReadAsJsonAsync<AuctionDto>();

        Assert.Equal(_auction.Seller, dto.Seller);
        Assert.Equal(_auction.Item.Make, dto.Make);
        Assert.Equal(_auction.Item.Model, dto.Model);
        Assert.Equal(_auction.Status.ToString(), dto.Status);
    }
    
    [Fact]
    public async Task GetAuction_WithInvalidId_Returns404()
    {
        await fixture.Host.Scenario(s =>
        {
            s.Get.Url($"/api/auctions/fake");
            s.StatusCodeShouldBe(HttpStatusCode.NotFound);
        });
    }
    
    [Fact]
    public async Task CreateAuction_WithoutAuth_Returns401()
    {
        var dto = TestData.BuildCreateAuctionDto();
        
        await fixture.Host.Scenario(s =>
        {
            s.RemoveRequestHeader("Authorization");
            s.Post.Json(dto).ToUrl("/api/auctions");
            s.StatusCodeShouldBe(HttpStatusCode.Unauthorized);
        });
    }
    
    [Fact]
    public async Task CreateAuction_WithAuth_ReturnsCreatedWithSellerSetFromUser()
    {
        var dto = TestData.BuildCreateAuctionDto();
        
        var result = await fixture.Host.Scenario(s =>
        {
            s.WithClaim(new Claim("username", "bob"));
            s.Post.Json(dto).ToUrl("/api/auctions");
            s.StatusCodeShouldBe(HttpStatusCode.Created);
        });
        
        var created = await result.ReadAsJsonAsync<AuctionDto>();

        Assert.Equal("bob", created.Seller);
        Assert.Equal(dto.Make, created.Make);
        Assert.Equal(dto.Model, created.Model);
    }
    
    [Fact]
    public async Task UpdateAuction_WithData_Returns204()
    {
        var updateDto = new UpdateAuctionDto{Model = "Updated"};
        
        await fixture.Host.Scenario(s =>
        {
            s.WithClaim(new Claim("username", "seller"));
            s.Put.Json(updateDto).ToUrl($"/api/auctions/{_auction.Id}");
            s.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        
        var auction = await dbContext.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == _auction.Id);
        
        Assert.Equal("Updated", auction!.Item.Model);
    }
    
    [Fact]
    public async Task DeleteAuction_WithValidId_Returns204()
    {
        await fixture.Host.Scenario(s =>
        {
            s.WithClaim(new Claim("username", "seller"));
            s.Delete.Url($"/api/auctions/{_auction.Id}");
            s.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });
        
        using var scope = fixture.Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        
        var auction = await dbContext.Auctions.FindAsync(_auction.Id);
        
        Assert.Null(auction);
    }
}