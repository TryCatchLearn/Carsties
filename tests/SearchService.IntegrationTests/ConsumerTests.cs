using AutoFixture;
using Contracts;
using MassTransit.Testing;
using MongoDB.Entities;

namespace SearchService.IntegrationTests;

public class ConsumerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly ITestHarness _testHarness;
    private readonly Fixture _fixture;

    public ConsumerTests(CustomWebAppFactory factory)
    {
        _testHarness = factory.Services.GetTestHarness();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task AuctionCreated_ShouldCreateItemInDb()
    {
        // arrange
        var consumerHarness = _testHarness.GetConsumerHarness<AuctionCreatedConsumer>();
        var auction = _fixture.Create<AuctionCreated>();
        
        // act
        await _testHarness.Bus.Publish(auction);
        
        // assert
        Assert.True(await consumerHarness.Consumed.Any<AuctionCreated>());
        var item = await DB.Find<Item>().OneAsync(auction.Id.ToString());
        Assert.Equal(auction.Make, item.Make);
    }
}